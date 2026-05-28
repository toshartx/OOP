using Forecast.Clients;

namespace Forecast.Services;

public class WeatherProviderFactory
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
        IWeatherDataClient client;
        switch (providerName) 
        {
            case "openweather":
                client = _openWeather;
                break;
            case "googleweather":
                client = _googleWeather;
                break;
            default:
                throw new ArgumentException($"Unknown Provider: {providerName}");
        }
        return client;   
    }
}
