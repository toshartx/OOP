using System.Globalization;
using System.Text.Json.Serialization;
using Forecast.Models.Weather;
using Forecast.Utils;

namespace Forecast.Clients;

public class OpenWeatherDataClient : IWeatherDataClient
{
    private readonly HttpClient client;
    private readonly string apiKey;

    public OpenWeatherDataClient(IConfiguration config, HttpClient httpClient)
    {
        client = httpClient;
        client.BaseAddress = new Uri(config.GetValue<string>("OPENWEATHER_BASE_URL") ?? "");
        apiKey = config.GetValue<string>("OPENWEATHER_API_KEY") ?? "";
    }

    public async Task<decimal> LocationCurrentTemperature(decimal latitude, decimal longitude)
    {
        try
        {
            var latStr = latitude.ToString(CultureInfo.InvariantCulture);
            var lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            var response = await client.GetAsync(
                $"/weather?lat={latStr}&lon={lonStr}&appid={apiKey}&units=metric"
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(
                    $"openweather returned bad status: {(ushort)response.StatusCode}"
                );
            }

            var data = await response.Content.ReadFromJsonAsync<OpenWeatherCurrentResponse>();
            return data?.Main?.Temp ?? throw new ApiCallException("failed to decode response");
        }
        catch (HttpRequestException e)
        {
            throw new ApiCallException($"failed to call openweather: {e.Message}", e);
        }
    }

    public async Task<List<ForecastDay>> LocationForecast(
        decimal latitude, decimal longitude, int days)
    {
        try
        {
            var latStr = latitude.ToString(CultureInfo.InvariantCulture);
            var lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            var response = await client.GetAsync(
                $"/forecast?lat={latStr}&lon={lonStr}&appid={apiKey}&units=metric&cnt={days * 8}"
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(
                    $"openweather forecast returned bad status: {(ushort)response.StatusCode}"
                );
            }

            var data = await response.Content.ReadFromJsonAsync<OpenWeatherForecastResponse>();

            if (data?.List == null || data.List.Count == 0)
                throw new ApiCallException("failed to decode forecast response");

            // Группируем по дням и берем среднюю температуру
            var dailyForecast = data.List
                .GroupBy(item => DateTime.Parse(item.DtTxt).Date)
                .Select(group => new ForecastDay(
                    group.Key,
                    Math.Round(group.Average(item => item.Main.Temp), 1),
                    group.First().Weather.First().Description
                ))
                .Take(days)
                .ToList();

            return dailyForecast;
        }
        catch (HttpRequestException e)
        {
            throw new ApiCallException($"failed to call openweather forecast: {e.Message}", e);
        }
    }
}

// Модели для OpenWeather API
class OpenWeatherCurrentResponse
{
    [JsonPropertyName("main")]
    public required MainData Main { get; set; }

    public class MainData
    {
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }
    }
}

class OpenWeatherForecastResponse
{
    [JsonPropertyName("list")]
    public required List<ForecastItem> List { get; set; }
}

class ForecastItem
{
    [JsonPropertyName("main")]
    public required MainData Main { get; set; }

    [JsonPropertyName("weather")]
    public required List<WeatherData> Weather { get; set; }

    [JsonPropertyName("dt_txt")]
    public required string DtTxt { get; set; }

    public class MainData
    {
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }
    }

    public class WeatherData
    {
        [JsonPropertyName("description")]
        public required string Description { get; set; }
    }
}