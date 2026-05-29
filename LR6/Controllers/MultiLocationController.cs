using System.Globalization;
using Forecast.Models.Weather;
using Forecast.Services;

namespace Forecast.Controllers;

public class MultiLocationController
{
    private readonly IWeatherProviderFactory _factory;
    private readonly CityCoordinatesService _cityService;

    public MultiLocationController(
        IWeatherProviderFactory factory,
        CityCoordinatesService cityService)
    {
        _factory = factory;
        _cityService = cityService;
    }

    public async Task<List<LocationTemperature>> GetMultiLocationTemperature(
        List<string> locations,
        string provider = "openweather")
    {
        var client = _factory.GetProvider(provider);
        var results = new List<LocationTemperature>();

        foreach (var location in locations)
        {
            decimal latitude;
            decimal longitude;
            string locationName;

            // Пробуем распарсить как координаты
            if (location.Contains(','))
            {
                var parts = location.Split(',');
                latitude = decimal.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
                longitude = decimal.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
                locationName = location;
            }
            else
            {
                // Ищем как город
                (latitude, longitude) = _cityService.GetCoordinates(location);
                locationName = location;
            }

            var temperature = await client.LocationCurrentTemperature(latitude, longitude);
            results.Add(new LocationTemperature(locationName, temperature));
        }

        return results;
    }
}