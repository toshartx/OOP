using Xunit;
using Forecast.Services;

namespace Tests;

public class CityCoordinatesServiceTests
{
    // Тест 1: Проверяем каждый город из задания
    [Theory]
    [InlineData("Минск", 53.9, 27.5)]
    [InlineData("Лондон", 51.5, -0.1)]
    [InlineData("Токио", 35.7, 139.7)]
    [InlineData("Шанхай", 31.2, 121.5)]
    [InlineData("Варшава", 52.2, 21.0)]
    public void GetCoordinates_ForKnownCity_ReturnsCorrectCoordinates(
        string city, double expectedLat, double expectedLon)
    {
        // Arrange
        var service = new CityCoordinatesService();

        // Act
        var (latitude, longitude) = service.GetCoordinates(city);

        // Assert
        Assert.Equal((decimal)expectedLat, latitude);
        Assert.Equal((decimal)expectedLon, longitude);
    }

    // Тест 2: Разный регистр букв должен работать
    [Theory]
    [InlineData("МИНСК")]
    [InlineData("Минск")]
    [InlineData("минск")]
    public void GetCoordinates_CaseInsensitive_ReturnsSameResult(string city)
    {
        // Arrange
        var service = new CityCoordinatesService();

        // Act
        var (latitude, longitude) = service.GetCoordinates(city);

        // Assert
        Assert.Equal(53.9m, latitude);
        Assert.Equal(27.5m, longitude);
    }

    // Тест 3: Неизвестный город — исключение
    [Fact]
    public void GetCoordinates_ForUnknownCity_ThrowsArgumentException()
    {
        // Arrange
        var service = new CityCoordinatesService();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => service.GetCoordinates("Нью-Йорк")
        );

        Assert.Contains("Нью-Йорк", exception.Message);
    }

    [Fact]
    public void GetCoordinates_AllCities_ReturnValidCoordinates()
    {
        var service = new CityCoordinatesService();

        var cities = new[] { "Минск", "Лондон", "Токио", "Шанхай", "Варшава" };

        foreach (var city in cities)
        {
            var (lat, lon) = service.GetCoordinates(city);
            Assert.NotEqual(0m, lat);
            Assert.NotEqual(0m, lon);
        }
    }
}