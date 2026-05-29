using Moq;
using Xunit;
using Forecast.Clients;
using Forecast.Controllers;
using Forecast.Models.Weather;
using Forecast.Services;

namespace Tests;

public class MultiLocationControllerTests
{
    // Тест 1: Получение температуры для нескольких городов
    [Fact]
    public async Task GetMultiLocationTemperature_WithCities_ReturnsTemperatures()
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();

        // Настраиваем разные температуры для разных координат
        mockClient
            .Setup(c => c.LocationCurrentTemperature(53.9m, 27.5m))
            .ReturnsAsync(5.0m);  // Минск

        mockClient
            .Setup(c => c.LocationCurrentTemperature(51.5m, -0.1m))
            .ReturnsAsync(10.0m); // Лондон

        mockClient
            .Setup(c => c.LocationCurrentTemperature(35.7m, 139.7m))
            .ReturnsAsync(15.0m); // Токио

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider(It.IsAny<string>()))
            .Returns(mockClient.Object);

        var cityService = new CityCoordinatesService();
        var controller = new MultiLocationController(mockFactory.Object, cityService);

        var locations = new List<string> { "Минск", "Лондон", "Токио" };

        // Act
        var results = await controller.GetMultiLocationTemperature(locations);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(3, results.Count);

        Assert.Equal("Минск", results[0].Location);
        Assert.Equal(5.0m, results[0].Temperature);

        Assert.Equal("Лондон", results[1].Location);
        Assert.Equal(10.0m, results[1].Temperature);

        Assert.Equal("Токио", results[2].Location);
        Assert.Equal(15.0m, results[2].Temperature);
    }

    // Тест 2: Смешанные локации (города и координаты)
    [Fact]
    public async Task GetMultiLocationTemperature_WithMixedLocations_ReturnsTemperatures()
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient
            .Setup(c => c.LocationCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .ReturnsAsync(20.0m);

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider(It.IsAny<string>()))
            .Returns(mockClient.Object);

        var cityService = new CityCoordinatesService();
        var controller = new MultiLocationController(mockFactory.Object, cityService);

        var locations = new List<string>
        {
            "Минск",                    // Город
            "53.9,27.5",               // Координаты
            "Варшава"                   // Город
        };

        // Act
        var results = await controller.GetMultiLocationTemperature(locations);

        // Assert
        Assert.Equal(3, results.Count);
        Assert.All(results, r => Assert.Equal(20.0m, r.Temperature));
    }

    // Тест 3: Обработка ошибок в одной из локаций
    [Fact]
    public async Task GetMultiLocationTemperature_WithInvalidCity_ThrowsException()
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();
        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider(It.IsAny<string>()))
            .Returns(mockClient.Object);

        var cityService = new CityCoordinatesService();
        var controller = new MultiLocationController(mockFactory.Object, cityService);

        var locations = new List<string> { "Минск", "НесуществующийГород" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => controller.GetMultiLocationTemperature(locations)
        );
    }

    [Fact]
    public async Task GetMultiLocationTemperature_WithGoogleProvider_Works()
    {
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient.Setup(c => c.LocationCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .ReturnsAsync(12m);

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory.Setup(f => f.GetProvider("googleweather")).Returns(mockClient.Object);

        var cityService = new CityCoordinatesService();
        var controller = new MultiLocationController(mockFactory.Object, cityService);

        var result = await controller.GetMultiLocationTemperature(
            new List<string> { "Токио", "Шанхай" }, "googleweather");

        Assert.Equal(2, result.Count);
        mockFactory.Verify(f => f.GetProvider("googleweather"), Times.Once);
    }

    [Fact]
    public async Task GetMultiLocationTemperature_WithCoordinates_Works()
    {
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient.Setup(c => c.LocationCurrentTemperature(53.9m, 27.5m)).ReturnsAsync(10m);
        mockClient.Setup(c => c.LocationCurrentTemperature(51.5m, -0.1m)).ReturnsAsync(15m);

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory.Setup(f => f.GetProvider(It.IsAny<string>())).Returns(mockClient.Object);

        var cityService = new CityCoordinatesService();
        var controller = new MultiLocationController(mockFactory.Object, cityService);

        var result = await controller.GetMultiLocationTemperature(
            new List<string> { "53.9,27.5", "51.5,-0.1" });

        Assert.Equal(2, result.Count);
        Assert.Equal(10m, result[0].Temperature);
        Assert.Equal(15m, result[1].Temperature);
    }
}