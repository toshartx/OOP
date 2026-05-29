using Moq;
using Xunit;
using Forecast.Clients;
using Forecast.Utils;
using System.Net;
using Moq.Protected;
using Microsoft.Extensions.Configuration;

namespace Tests;

public class OpenWeatherDataClientTests
{
    private Mock<IConfiguration> CreateConfigMock(string baseUrl = "https://test.com/", string apiKey = "test-key")
    {
        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value).Returns(baseUrl);
        apiKeySectionMock.Setup(s => s.Value).Returns(apiKey);

        configMock.Setup(c => c.GetSection("OPENWEATHER_BASE_URL")).Returns(baseUrlSectionMock.Object);
        configMock.Setup(c => c.GetSection("OPENWEATHER_API_KEY")).Returns(apiKeySectionMock.Object);

        return configMock;
    }

    [Fact]
    public async Task LocationCurrentTemperature_ValidResponse_ReturnsTemperature()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"main\":{\"temp\":15.5}}")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenWeatherDataClient(CreateConfigMock().Object, httpClient);
        var temp = await client.LocationCurrentTemperature(0, 0);

        Assert.Equal(15.5m, temp);
    }

    [Fact]
    public async Task LocationCurrentTemperature_BadStatus_ThrowsException()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenWeatherDataClient(CreateConfigMock().Object, httpClient);

        await Assert.ThrowsAsync<ApiCallException>(() => client.LocationCurrentTemperature(0, 0));
    }

    [Fact]
    public async Task LocationCurrentTemperature_HttpError_ThrowsApiCallException()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenWeatherDataClient(CreateConfigMock().Object, httpClient);

        var ex = await Assert.ThrowsAsync<ApiCallException>(() => client.LocationCurrentTemperature(0, 0));
        Assert.Contains("Network error", ex.Message);
    }

    [Fact]
    public async Task LocationForecast_ValidResponse_ReturnsForecast()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                    ""list"": [
                        {""main"":{""temp"":10},""weather"":[{""description"":""clear""}],""dt_txt"":""2024-01-15 12:00:00""},
                        {""main"":{""temp"":12},""weather"":[{""description"":""cloudy""}],""dt_txt"":""2024-01-16 12:00:00""}
                    ]
                }")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenWeatherDataClient(CreateConfigMock().Object, httpClient);
        var forecast = await client.LocationForecast(0, 0, 2);

        Assert.Equal(2, forecast.Count);
        Assert.Equal(10m, forecast[0].Temperature);
        Assert.Equal("clear", forecast[0].Condition);
    }

    [Fact]
    public async Task LocationForecast_BadStatus_ThrowsException()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenWeatherDataClient(CreateConfigMock().Object, httpClient);

        await Assert.ThrowsAsync<ApiCallException>(() => client.LocationForecast(0, 0, 3));
    }

    [Fact]
    public async Task LocationForecast_HttpError_ThrowsException()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenWeatherDataClient(CreateConfigMock().Object, httpClient);

        var ex = await Assert.ThrowsAsync<ApiCallException>(() => client.LocationForecast(0, 0, 3));
        Assert.Contains("Network error", ex.Message);
    }
}