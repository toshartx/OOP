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
    // Тест 1: Успешный запрос к Google Weather API
    [Fact]
    public async Task LocationCurrentTemperature_ValidResponse_ReturnsTemperature()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        var responseJson = "{\"current\":{\"temp\":23.5}}";

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

        // Настраиваем IConfiguration через индексатор, а не GetValue
        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value)
            .Returns("https://api.google.com/weather");
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

        baseUrlSectionMock.Setup(s => s.Value).Returns("https://test.com");
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

    // Тест 3: Проверка формата запроса
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
                Content = new StringContent("{\"current\":{\"temp\":10.0}}")
            });

        var httpClient = new HttpClient(mockHandler.Object);

        var configMock = new Mock<IConfiguration>();
        var baseUrlSectionMock = new Mock<IConfigurationSection>();
        var apiKeySectionMock = new Mock<IConfigurationSection>();

        baseUrlSectionMock.Setup(s => s.Value)
            .Returns("https://api.google.com/weather");
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
        Assert.Contains("lat=53.9", capturedRequest!.RequestUri!.Query);
        Assert.Contains("lon=27.5", capturedRequest.RequestUri.Query);
        Assert.Contains("key=my-api-key", capturedRequest.RequestUri.Query);
    }
}