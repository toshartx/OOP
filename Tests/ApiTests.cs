using Moq;
using Xunit;
using Forecast.Api;
using Forecast.Controllers;
using Forecast.Models.Weather;
using Forecast.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;

namespace Tests;

public class ApiTests
{
    [Fact]
    public async Task WeatherApi_ValidRequest_ReturnsOk()
    {
        var mockFactory = new Mock<IWeatherProviderFactory>();
        var mockClient = new Mock<Forecast.Clients.IWeatherDataClient>();
        mockClient.Setup(c => c.LocationCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .ReturnsAsync(20m);
        mockFactory.Setup(f => f.GetProvider("openweather")).Returns(mockClient.Object);

        var controller = new CurrentWeatherController(mockFactory.Object);
        var cityService = new CityCoordinatesService();

        // Тестируем через контроллер напрямую (API сложно тестировать)
        var result = await controller.GetCurrentWeather(53.9m, 27.5m, "openweather");
        Assert.Equal(20m, result.Temperature);
    }

    [Fact]
    public async Task ForecastApi_ValidRequest_ReturnsForecast()
    {
        var mockFactory = new Mock<IWeatherProviderFactory>();
        var mockClient = new Mock<Forecast.Clients.IWeatherDataClient>();
        mockClient.Setup(c => c.LocationForecast(It.IsAny<decimal>(), It.IsAny<decimal>(), 3))
            .ReturnsAsync(new List<ForecastDay>
            {
                new(DateTime.Today, 10m, "Sunny"),
                new(DateTime.Today.AddDays(1), 12m, "Cloudy"),
                new(DateTime.Today.AddDays(2), 15m, "Rain")
            });
        mockFactory.Setup(f => f.GetProvider(It.IsAny<string>())).Returns(mockClient.Object);

        var controller = new ForecastController(mockFactory.Object);
        var result = await controller.GetForecast(0, 0, 3);

        Assert.Equal(3, result.Days.Count);
        Assert.Equal("Sunny", result.Days[0].Condition);
    }

    [Fact]
    public async Task MultiLocationApi_ValidRequest_ReturnsResults()
    {
        var mockFactory = new Mock<IWeatherProviderFactory>();
        var mockClient = new Mock<Forecast.Clients.IWeatherDataClient>();
        mockClient.Setup(c => c.LocationCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .ReturnsAsync(15m);
        mockFactory.Setup(f => f.GetProvider(It.IsAny<string>())).Returns(mockClient.Object);

        var cityService = new CityCoordinatesService();
        var controller = new MultiLocationController(mockFactory.Object, cityService);

        var result = await controller.GetMultiLocationTemperature(
            new List<string> { "Минск", "Лондон" });

        Assert.Equal(2, result.Count);
        Assert.Equal("Минск", result[0].Location);
        Assert.Equal("Лондон", result[1].Location);
    }
}