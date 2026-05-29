namespace Forecast.Services;

public class CityCoordinatesService
{
    private readonly Dictionary<string, (decimal Latitude, decimal Longitude)> _cities;

    public CityCoordinatesService()
    {
        _cities = new Dictionary<string, (decimal, decimal)>(
            StringComparer.OrdinalIgnoreCase)
        {
            ["Минск"] = (53.9m, 27.5m),
            ["Лондон"] = (51.5m, -0.1m),
            ["Токио"] = (35.7m, 139.7m),
            ["Шанхай"] = (31.2m, 121.5m),
            ["Варшава"] = (52.2m, 21.0m)
        };
    }

    public (decimal Latitude, decimal Longitude) GetCoordinates(string city)
    {
        if (_cities.TryGetValue(city, out var coordinates))
        {
            return coordinates;
        }

        throw new ArgumentException($"Unknown city: {city}");
    }
}