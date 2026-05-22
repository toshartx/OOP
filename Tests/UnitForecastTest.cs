using Forecast.Clients;
using Forecast.Controllers;
using Forecast.Models.Weather;
using Moq;

namespace Tests
{
    public class UnitForecastTest
    {
        [Fact]
        public async Task DefaultTest()
        {
            // Подготовка
            var mockClient = new Mock<IWeatherDataClient>();
            mockClient.Setup(client => client.LocationCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>())).ReturnsAsync(22.5m);
            var controller = new CurrentWeatherController(mockClient.Object);

            // Действие
            var result = await controller.GetCurrentWeather(22.5m, -11.2m);

            // Проверка результата
            Assert.Equal(new CurrentWeather(22.5m), result);
        }

        [Fact]
        public async Task GetProvider_WithOpenWeather_ReturnsOpenWeatherClient()
        {
            var openWeather = new Mock<IWeatherDataClient>();
            var googleWeather = new Mock<IWeatherDataClient>();
            var factory = new WeatherProviderFactory(openWeather.Object, googleWeather.Object);

            var result = factory.GetProvider("openweather");

            Assert.Same(openWeather.Object, result);
        }

        [Fact]
        public void GetProvider_WithGoogleWeather_ReturnsGoogleWeatherClient()
        {
            var openWeatherClient = new Mock<IWeatherDataClient>();
            var googleWeatherClient = new Mock<IWeatherDataClient>();

            var factory = new WeatherProviderFactory(
                openWeatherClient.Object,
                googleWeatherClient.Object);

            var provider = factory.GetProvider("googleweather");

            Assert.Same(googleWeatherClient.Object, provider);
        }

        
        [Fact]
        public void GetProvider_WithUnknownProvider_ThrowsException()
        {
            
            var openWeatherClient = new Mock<IWeatherDataClient>();
            var googleWeatherClient = new Mock<IWeatherDataClient>();

            var factory = new WeatherProviderFactory(
                openWeatherClient.Object,
                googleWeatherClient.Object);

            var exception = Assert.Throws<ArgumentException>(
                () => factory.GetProvider("unknown_provider")
            );

            Assert.Contains("unknown_provider", exception.Message);
        }
    }
}
