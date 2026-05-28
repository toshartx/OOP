using Moq;
using Xunit;
using Forecast.Controllers;
using Forecast.Clients;
using Forecast.Services;
using Forecast.Models.Weather;

namespace Tests;

public class CurrentWeatherControllerTests
{
    [Fact]
    public async Task GetCurrentWeather_UsesFactoryToGetProvider()
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient
            .Setup(c => c.LocationCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .ReturnsAsync(15.0m);

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider("openweather"))
            .Returns(mockClient.Object);

        var controller = new CurrentWeatherController(mockFactory.Object);

        // Act
        var result = await controller.GetCurrentWeather(53.9m, 27.5m, "openweather");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15.0m, result.Temperature);

        mockFactory.Verify(f => f.GetProvider("openweather"), Times.Once);
        mockClient.Verify(c => c.LocationCurrentTemperature(53.9m, 27.5m), Times.Once);
    }

    [Theory]
    [InlineData("openweather")]
    [InlineData("googleweather")]
    public async Task GetCurrentWeather_WorksWithDifferentProviders(string providerName)
    {
        // Arrange
        var mockClient = new Mock<IWeatherDataClient>();
        mockClient
            .Setup(c => c.LocationCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .ReturnsAsync(20.0m);

        var mockFactory = new Mock<IWeatherProviderFactory>();
        mockFactory
            .Setup(f => f.GetProvider(providerName))
            .Returns(mockClient.Object);

        var controller = new CurrentWeatherController(mockFactory.Object);

        // Act
        var result = await controller.GetCurrentWeather(0, 0, providerName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(20.0m, result.Temperature);
        mockFactory.Verify(f => f.GetProvider(providerName), Times.Once);
    }
}