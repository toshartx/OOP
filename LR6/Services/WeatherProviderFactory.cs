using Forecast.Clients;

namespace Forecast.Services;

public class WeatherProviderFactory : IWeatherProviderFactory
{
    private readonly IWeatherDataClient _openWeather;
    private readonly IWeatherDataClient _googleWeather;

    public WeatherProviderFactory(
        IWeatherDataClient openWeather,
        IWeatherDataClient googleWeather)
    {
        _openWeather = openWeather;
        _googleWeather = googleWeather;
    }

    public IWeatherDataClient GetProvider(string providerName)
    {
        return providerName.ToLower() switch
        {
            "openweather" => _openWeather,
            "googleweather" => _googleWeather,
            _ => throw new ArgumentException($"Unknown provider: {providerName}")
        };
    }
}