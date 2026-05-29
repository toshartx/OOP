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
            // Используем CultureInfo.InvariantCulture для форматирования с точкой
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
                .ReadFromJsonAsync<GoogleWeatherResponse>();

            return data?.Current?.Temp
                ?? throw new ApiCallException("failed to decode response");
        }
        catch (HttpRequestException e)
        {
            throw new ApiCallException(
                $"failed to call google weather: {e.Message}", e);
        }
    }

    public Task<List<ForecastDay>> LocationForecast(decimal latitude, decimal longitude, int days)
    {
        throw new NotImplementedException();
    }
}

public class GoogleWeatherResponse
{
    [JsonPropertyName("current")]
    public required CurrentData Current { get; set; }

    public class CurrentData
    {
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }
    }
}