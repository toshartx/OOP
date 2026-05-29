using System.Globalization;
using System.Text.Json.Serialization;
using Forecast.Models.Weather;
using Forecast.Utils;

namespace Forecast.Clients;

public class GoogleWeatherDataClient : IWeatherDataClient
{
    private readonly HttpClient _client;
    private readonly string _apiKey;

    public GoogleWeatherDataClient(IConfiguration config, HttpClient httpClient)
    {
        _client = httpClient;
        _client.BaseAddress = new Uri(
            config.GetValue<string>("GOOGLE_WEATHER_BASE_URL") ?? "");
        _apiKey = config.GetValue<string>("GOOGLE_WEATHER_API_KEY") ?? "";
    }

    public async Task<decimal> LocationCurrentTemperature(
        decimal latitude, decimal longitude)
    {
        try
        {
            var latStr = latitude.ToString(CultureInfo.InvariantCulture);
            var lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            var response = await _client.GetAsync(
                $"?lat={latStr}&lon={lonStr}&key={_apiKey}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(
                    $"google weather returned bad status: {(ushort)response.StatusCode}");
            }

            var data = await response.Content
                .ReadFromJsonAsync<GoogleWeatherCurrentResponse>();

            return data?.Current?.Temp
                ?? throw new ApiCallException("failed to decode response");
        }
        catch (HttpRequestException e)
        {
            throw new ApiCallException(
                $"failed to call google weather: {e.Message}", e);
        }
    }

    public async Task<List<ForecastDay>> LocationForecast(
        decimal latitude, decimal longitude, int days)
    {
        try
        {
            var latStr = latitude.ToString(CultureInfo.InvariantCulture);
            var lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            var response = await _client.GetAsync(
                $"/forecast?lat={latStr}&lon={lonStr}&key={_apiKey}&days={days}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(
                    $"google weather forecast returned bad status: {(ushort)response.StatusCode}");
            }

            var data = await response.Content
                .ReadFromJsonAsync<GoogleWeatherForecastResponse>();

            if (data?.Daily == null || data.Daily.Count == 0)
                throw new ApiCallException("failed to decode forecast response");

            var forecastDays = data.Daily
                .Select(day => new ForecastDay(
                    DateTime.Parse(day.Date),
                    day.Temp.Day,
                    day.Weather.First().Description
                ))
                .Take(days)
                .ToList();

            return forecastDays;
        }
        catch (HttpRequestException e)
        {
            throw new ApiCallException(
                $"failed to call google weather forecast: {e.Message}", e);
        }
    }
}

// Модели для Google Weather API
class GoogleWeatherCurrentResponse
{
    [JsonPropertyName("current")]
    public required CurrentData Current { get; set; }

    public class CurrentData
    {
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }
    }
}

class GoogleWeatherForecastResponse
{
    [JsonPropertyName("daily")]
    public required List<DailyData> Daily { get; set; }
}

class DailyData
{
    [JsonPropertyName("dt")]
    public required string Date { get; set; }

    [JsonPropertyName("temp")]
    public required TempData Temp { get; set; }

    [JsonPropertyName("weather")]
    public required List<WeatherInfo> Weather { get; set; }
}

class TempData
{
    [JsonPropertyName("day")]
    public decimal Day { get; set; }
}

class WeatherInfo
{
    [JsonPropertyName("description")]
    public required string Description { get; set; }
}