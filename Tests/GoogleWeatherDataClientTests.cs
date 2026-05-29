using Moq;
using Xunit;
using Forecast.Clients;
using Forecast.Utils;
using System.Net;
using Moq.Protected;
using Microsoft.Extensions.Configuration;

namespace Tests;

public class GoogleWeatherDataClientTests
{
    private Mock<IConfiguration> CreateConfigMock(string baseUrl = "https://weather.googleapis.com/v1/", string apiKey = "test-key")
    {
        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value).Returns(baseUrl);
        apiKeySectionMock.Setup(s => s.Value).Returns(apiKey);

        configMock.Setup(c => c.GetSection("GOOGLE_WEATHER_BASE_URL")).Returns(baseUrlSectionMock.Object);
        configMock.Setup(c => c.GetSection("GOOGLE_WEATHER_API_KEY")).Returns(apiKeySectionMock.Object);

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
                Content = new StringContent(@"{""temperature"":{""degrees"":23.5}}")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new GoogleWeatherDataClient(CreateConfigMock().Object, httpClient);
        var temp = await client.LocationCurrentTemperature(53.9m, 27.5m);

        Assert.Equal(23.5m, temp);
    }

    [Fact]
    public async Task LocationCurrentTemperature_BadStatus_ThrowsApiCallException()
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
        var client = new GoogleWeatherDataClient(CreateConfigMock().Object, httpClient);

        await Assert.ThrowsAsync<ApiCallException>(() => client.LocationCurrentTemperature(0, 0));
    }

    [Fact]
    public async Task LocationCurrentTemperature_HttpError_ThrowsException()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Google API error"));

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new GoogleWeatherDataClient(CreateConfigMock().Object, httpClient);

        var ex = await Assert.ThrowsAsync<ApiCallException>(() => client.LocationCurrentTemperature(0, 0));
        Assert.Contains("Google API error", ex.Message);
    }

    [Fact]
    public async Task LocationCurrentTemperature_SendsCorrectRequest()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""temperature"":{""degrees"":10.0}}")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new GoogleWeatherDataClient(CreateConfigMock().Object, httpClient);

        await client.LocationCurrentTemperature(53.9m, 27.5m);

        Assert.NotNull(capturedRequest);
        Assert.Contains("currentConditions:lookup", capturedRequest!.RequestUri!.AbsoluteUri);
        Assert.Contains("key=test-key", capturedRequest.RequestUri.Query);
    }

    [Fact]
    public async Task LocationForecast_ValidResponse_ReturnsForecastDays()
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
                    ""forecastDays"": [
                        {
                            ""displayDate"": {""year"": 2024, ""month"": 1, ""day"": 15},
                            ""maxTemperature"": {""degrees"": 25.0},
                            ""minTemperature"": {""degrees"": 20.0},
                            ""daytimeForecast"": {
                                ""weatherCondition"": {
                                    ""description"": {""text"": ""Sunny""}
                                }
                            }
                        },
                        {
                            ""displayDate"": {""year"": 2024, ""month"": 1, ""day"": 16},
                            ""maxTemperature"": {""degrees"": 20.0},
                            ""minTemperature"": {""degrees"": 16.0},
                            ""daytimeForecast"": {
                                ""weatherCondition"": {
                                    ""description"": {""text"": ""Cloudy""}
                                }
                            }
                        }
                    ]
                }")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new GoogleWeatherDataClient(CreateConfigMock().Object, httpClient);
        var forecast = await client.LocationForecast(53.9m, 27.5m, 2);

        Assert.Equal(2, forecast.Count);
        Assert.Equal(22.5m, forecast[0].Temperature);
        Assert.Equal("Sunny", forecast[0].Condition);
        Assert.Equal(18.0m, forecast[1].Temperature);
        Assert.Equal("Cloudy", forecast[1].Condition);
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
                StatusCode = HttpStatusCode.NotFound
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new GoogleWeatherDataClient(CreateConfigMock().Object, httpClient);

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
        var client = new GoogleWeatherDataClient(CreateConfigMock().Object, httpClient);

        var ex = await Assert.ThrowsAsync<ApiCallException>(() => client.LocationForecast(0, 0, 3));
        Assert.Contains("Network error", ex.Message);
    }
}