using System.Globalization;
using System.Text.Json;
using Forecast.Models.Weather;
using Forecast.Utils;

namespace Forecast.Clients;

public class GoogleWeatherDataClient : IWeatherDataClient
{
    private readonly HttpClient _client;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private static readonly Random Random = new();

    public GoogleWeatherDataClient(IConfiguration config, HttpClient httpClient)
    {
        _client = httpClient;
        _baseUrl = config.GetValue<string>("GOOGLE_WEATHER_BASE_URL") ?? "";
        _apiKey = config.GetValue<string>("GOOGLE_WEATHER_API_KEY") ?? "";
    }

    public async Task<decimal> LocationCurrentTemperature(decimal latitude, decimal longitude)
    {
        try
        {
            var latStr = latitude.ToString(CultureInfo.InvariantCulture);
            var lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            var url = $"{_baseUrl}currentConditions:lookup?key={_apiKey}&location.latitude={latStr}&location.longitude={lonStr}";

            var response = await _client.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(
                    $"google weather returned bad status: {(ushort)response.StatusCode}");
            }

            using var json = JsonDocument.Parse(responseBody);
            var temp = json.RootElement.GetProperty("temperature").GetProperty("degrees").GetDecimal();
            return temp;
        }
        catch (HttpRequestException e)
        {
            throw new ApiCallException($"failed to call google weather: {e.Message}", e);
        }
    }

    public async Task<List<ForecastDay>> LocationForecast(decimal latitude, decimal longitude, int days)
    {
        try
        {
            var latStr = latitude.ToString(CultureInfo.InvariantCulture);
            var lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            // Правильный URL для прогноза
            var url = $"{_baseUrl}forecast/days:lookup?key={_apiKey}&location.latitude={latStr}&location.longitude={lonStr}&days={days}";

            var response = await _client.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(
                    $"google weather forecast returned bad status: {(ushort)response.StatusCode}");
            }

            using var json = JsonDocument.Parse(responseBody);
            var daysArray = json.RootElement.GetProperty("forecastDays");

            var forecast = new List<ForecastDay>();
            foreach (var day in daysArray.EnumerateArray())
            {
                var displayDate = day.GetProperty("displayDate");
                var year = displayDate.GetProperty("year").GetInt32();
                var month = displayDate.GetProperty("month").GetInt32();
                var dayOfMonth = displayDate.GetProperty("day").GetInt32();
                var date = new DateTime(year, month, dayOfMonth);

                var maxTemp = day.GetProperty("maxTemperature").GetProperty("degrees").GetDecimal();
                var minTemp = day.GetProperty("minTemperature").GetProperty("degrees").GetDecimal();
                var avgTemp = (maxTemp + minTemp) / 2.0m;

                var condition = day.GetProperty("daytimeForecast")
                    .GetProperty("weatherCondition")
                    .GetProperty("description")
                    .GetProperty("text")
                    .GetString()!;

                forecast.Add(new ForecastDay(date, Math.Round(avgTemp, 1), condition));
            }

            return forecast.Take(days).ToList();
        }
        catch (HttpRequestException e)
        {
            throw new ApiCallException($"failed to call google weather forecast: {e.Message}", e);
        }
    }
}