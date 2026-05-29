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
    // Тест 1: Успешный запрос текущей температуры
    [Fact]
    public async Task LocationCurrentTemperature_ValidResponse_ReturnsTemperature()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        var responseJson = @"{
            ""temperature"": {
                ""degrees"": 23.5,
                ""unit"": ""CELSIUS""
            }
        }";

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            });

        var httpClient = new HttpClient(mockHandler.Object);

        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value)
            .Returns("https://weather.googleapis.com/v1/");
        apiKeySectionMock.Setup(s => s.Value)
            .Returns("test-key");

        configMock
            .Setup(c => c.GetSection("GOOGLE_WEATHER_BASE_URL"))
            .Returns(baseUrlSectionMock.Object);
        configMock
            .Setup(c => c.GetSection("GOOGLE_WEATHER_API_KEY"))
            .Returns(apiKeySectionMock.Object);

        var client = new GoogleWeatherDataClient(configMock.Object, httpClient);

        // Act
        var temperature = await client.LocationCurrentTemperature(53.9m, 27.5m);

        // Assert
        Assert.Equal(23.5m, temperature);
    }

    // Тест 2: Обработка ошибки при неуспешном статусе
    [Fact]
    public async Task LocationCurrentTemperature_BadStatus_ThrowsApiCallException()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            });

        var httpClient = new HttpClient(mockHandler.Object);

        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value).Returns("https://weather.googleapis.com/v1/");
        apiKeySectionMock.Setup(s => s.Value).Returns("test");

        configMock
            .Setup(c => c.GetSection("GOOGLE_WEATHER_BASE_URL"))
            .Returns(baseUrlSectionMock.Object);
        configMock
            .Setup(c => c.GetSection("GOOGLE_WEATHER_API_KEY"))
            .Returns(apiKeySectionMock.Object);

        var client = new GoogleWeatherDataClient(configMock.Object, httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<ApiCallException>(
            () => client.LocationCurrentTemperature(0, 0)
        );
    }

    // Тест 3: Проверка формата запроса текущей погоды
    [Fact]
    public async Task LocationCurrentTemperature_SendsCorrectRequest()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""temperature"":{""degrees"":10.0}}")
            });

        var httpClient = new HttpClient(mockHandler.Object);

        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value)
            .Returns("https://weather.googleapis.com/v1/");
        apiKeySectionMock.Setup(s => s.Value)
            .Returns("my-api-key");

        configMock
            .Setup(c => c.GetSection("GOOGLE_WEATHER_BASE_URL"))
            .Returns(baseUrlSectionMock.Object);
        configMock
            .Setup(c => c.GetSection("GOOGLE_WEATHER_API_KEY"))
            .Returns(apiKeySectionMock.Object);

        var client = new GoogleWeatherDataClient(configMock.Object, httpClient);

        // Act
        await client.LocationCurrentTemperature(53.9m, 27.5m);

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Contains("currentConditions:lookup", capturedRequest!.RequestUri!.AbsoluteUri);
        Assert.Contains("lat", capturedRequest.RequestUri.Query);
        Assert.Contains("lon", capturedRequest.RequestUri.Query);
        Assert.Contains("key=my-api-key", capturedRequest.RequestUri.Query);
    }

    // Тест 4: Успешный запрос прогноза
    [Fact]
    public async Task LocationForecast_ValidResponse_ReturnsForecastDays()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        var responseJson = @"{
            ""forecastDays"": [
                {
                    ""displayDate"": {
                        ""year"": 2024,
                        ""month"": 1,
                        ""day"": 15
                    },
                    ""maxTemperature"": {
                        ""degrees"": 25.0,
                        ""unit"": ""CELSIUS""
                    },
                    ""minTemperature"": {
                        ""degrees"": 20.0,
                        ""unit"": ""CELSIUS""
                    },
                    ""daytimeForecast"": {
                        ""weatherCondition"": {
                            ""description"": {
                                ""text"": ""Sunny"",
                                ""languageCode"": ""en""
                            }
                        }
                    }
                },
                {
                    ""displayDate"": {
                        ""year"": 2024,
                        ""month"": 1,
                        ""day"": 16
                    },
                    ""maxTemperature"": {
                        ""degrees"": 20.0,
                        ""unit"": ""CELSIUS""
                    },
                    ""minTemperature"": {
                        ""degrees"": 16.0,
                        ""unit"": ""CELSIUS""
                    },
                    ""daytimeForecast"": {
                        ""weatherCondition"": {
                            ""description"": {
                                ""text"": ""Cloudy"",
                                ""languageCode"": ""en""
                            }
                        }
                    }
                }
            ]
        }";

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            });

        var httpClient = new HttpClient(mockHandler.Object);

        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value).Returns("https://weather.googleapis.com/v1/");
        apiKeySectionMock.Setup(s => s.Value).Returns("test-key");

        configMock.Setup(c => c.GetSection("GOOGLE_WEATHER_BASE_URL"))
            .Returns(baseUrlSectionMock.Object);
        configMock.Setup(c => c.GetSection("GOOGLE_WEATHER_API_KEY"))
            .Returns(apiKeySectionMock.Object);

        var client = new GoogleWeatherDataClient(configMock.Object, httpClient);

        // Act
        var forecast = await client.LocationForecast(53.9m, 27.5m, 2);

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(2, forecast.Count);
        Assert.Equal(22.5m, forecast[0].Temperature); // (25+20)/2
        Assert.Equal("Sunny", forecast[0].Condition);
        Assert.Equal(18.0m, forecast[1].Temperature); // (20+16)/2
        Assert.Equal("Cloudy", forecast[1].Condition);
    }
}