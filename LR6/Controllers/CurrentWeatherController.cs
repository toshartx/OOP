using Forecast.Clients;
using Forecast.Models.Weather;
using Forecast.Services;

namespace Forecast.Controllers;

public class CurrentWeatherController
{
    private readonly IWeatherProviderFactory _factory;

    public CurrentWeatherController(IWeatherProviderFactory factory)
    {
        _factory = factory;
    }

    public async Task<CurrentWeather> GetCurrentWeather(
        decimal latitude,
        decimal longitude,
        string provider = "openweather")
    {
        var client = _factory.GetProvider(provider);
        var temperature = await client.LocationCurrentTemperature(latitude, longitude);
        return new CurrentWeather(temperature);
    }
}