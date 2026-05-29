using Moq;
using Xunit;
using Forecast.Clients;
using Forecast.Controllers;
using Forecast.Models.Weather;
using Forecast.Services;

namespace Tests;

public class ForecastControllerTests
{
    // Тест 1: Получение прогноза на несколько дней
    [Fact]
    public async Task GetForecast_ValidCoordinates_ReturnsForecastForMultipleDays()
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();

        // Настраиваем клиент для прогноза (будет возвращать список температур)
        mockClient
            .Setup(c => c.LocationForecast(
                It.IsAny<decimal>(),
                It.IsAny<decimal>(),
                It.IsAny<int>()))
            .ReturnsAsync(new List<ForecastDay>
            {
                new ForecastDay(DateTime.Today, 20.5m, "Sunny"),
                new ForecastDay(DateTime.Today.AddDays(1), 22.0m, "Cloudy"),
                new ForecastDay(DateTime.Today.AddDays(2), 18.3m, "Rainy")
            });

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider("openweather"))
            .Returns(mockClient.Object);

        var controller = new ForecastController(mockFactory.Object);

        // Act
        var forecast = await controller.GetForecast(53.9m, 27.5m, 3, "openweather");

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(3, forecast.Days.Count);
        Assert.Equal(20.5m, forecast.Days[0].Temperature);
        Assert.Equal("Sunny", forecast.Days[0].Condition);
        Assert.Equal(22.0m, forecast.Days[1].Temperature);
        Assert.Equal("Rainy", forecast.Days[2].Condition);
    }

    // Тест 2: Прогноз с выбором провайдера
    [Fact]
    public async Task GetForecast_UsesCorrectProvider()
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient
            .Setup(c => c.LocationForecast(It.IsAny<decimal>(), It.IsAny<decimal>(), 5))
            .ReturnsAsync(new List<ForecastDay>
            {
                new ForecastDay(DateTime.Today, 15.0m, "Clear")
            });

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider("googleweather"))
            .Returns(mockClient.Object);

        var controller = new ForecastController(mockFactory.Object);

        // Act
        var forecast = await controller.GetForecast(0, 0, 5, "googleweather");

        // Assert
        Assert.NotNull(forecast);
        mockFactory.Verify(f => f.GetProvider("googleweather"), Times.Once);
        mockClient.Verify(
            c => c.LocationForecast(0, 0, 5),
            Times.Once);
    }

    // Тест 3: Прогноз с городом
    [Fact]
    public async Task GetForecast_WithCity_ReturnsForecast()
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient
            .Setup(c => c.LocationForecast(53.9m, 27.5m, 3))
            .ReturnsAsync(new List<ForecastDay>
            {
                new ForecastDay(DateTime.Today, 10.0m, "Snow")
            });

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider(It.IsAny<string>()))
            .Returns(mockClient.Object);

        var controller = new ForecastController(mockFactory.Object);

        // Act
        var forecast = await controller.GetForecast(53.9m, 27.5m, 3);

        // Assert
        Assert.NotNull(forecast);
        Assert.Single(forecast.Days);
        Assert.Equal("Snow", forecast.Days[0].Condition);
    }

    [Fact]
    public async Task GetForecast_WithGoogleProvider_ReturnsCorrectData()
    {
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient.Setup(c => c.LocationForecast(It.IsAny<decimal>(), It.IsAny<decimal>(), 5))
            .ReturnsAsync(new List<ForecastDay>
            {
            new(DateTime.Today, 18m, "Fog"),
            new(DateTime.Today.AddDays(1), 19m, "Wind")
            });

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory.Setup(f => f.GetProvider("googleweather")).Returns(mockClient.Object);

        var controller = new ForecastController(mockFactory.Object);
        var result = await controller.GetForecast(0, 0, 5, "googleweather");

        Assert.Equal(2, result.Days.Count);
        Assert.Equal("Fog", result.Days[0].Condition);
        mockFactory.Verify(f => f.GetProvider("googleweather"), Times.Once);
    }

    [Fact]
    public async Task GetForecast_DefaultDays_IsSeven()
    {
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient.Setup(c => c.LocationForecast(It.IsAny<decimal>(), It.IsAny<decimal>(), 7))
            .ReturnsAsync(new List<ForecastDay>());

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory.Setup(f => f.GetProvider(It.IsAny<string>())).Returns(mockClient.Object);

        var controller = new ForecastController(mockFactory.Object);
        await controller.GetForecast(0, 0);

        mockClient.Verify(c => c.LocationForecast(0, 0, 7), Times.Once);
    }
}