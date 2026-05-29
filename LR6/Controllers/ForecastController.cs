using Forecast.Models.Weather;
using Forecast.Services;

namespace Forecast.Controllers;

public class ForecastController
{
    private readonly IWeatherProviderFactory _factory;

    public ForecastController(IWeatherProviderFactory factory)
    {
        _factory = factory;
    }

    public async Task<WeatherForecast> GetForecast(
        decimal latitude,
        decimal longitude,
        int days = 7,
        string provider = "openweather")
    {
        var client = _factory.GetProvider(provider);
        var forecastDays = await client.LocationForecast(latitude, longitude, days);
        return new WeatherForecast(forecastDays);
    }
}