using DotEnv.Core;
using Forecast.Api;
using Forecast.Clients;
using Forecast.Controllers;
using Forecast.Services;

var builder = WebApplication.CreateBuilder(args);

new EnvLoader().Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "WeatherExampleAPI";
    config.Title = "Weather Example API";
    config.Version = "v1";
});

// Регистрируем HttpClient для OpenWeatherDataClient
builder.Services.AddHttpClient<OpenWeatherDataClient>();

// Регистрируем GoogleWeatherDataClient (тоже через HttpClient)
builder.Services.AddHttpClient<GoogleWeatherDataClient>();

// Регистрируем сервисы
builder.Services.AddSingleton<CityCoordinatesService>();

// Регистрируем фабрику провайдеров
builder.Services.AddSingleton<IWeatherProviderFactory>(sp =>
{
    var openWeather = sp.GetRequiredService<OpenWeatherDataClient>();
    var googleWeather = sp.GetRequiredService<GoogleWeatherDataClient>();
    return new WeatherProviderFactory(openWeather, googleWeather);
});

// Регистрируем контроллеры
builder.Services.AddSingleton<CurrentWeatherController>();
builder.Services.AddSingleton<ForecastController>();
builder.Services.AddSingleton<MultiLocationController>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseDeveloperExceptionPage();
}

app.MapGroup("/api/v1").MapCurrentWeatherApi();
app.MapGroup("/api/v1").MapForecastApi();
app.MapGroup("/api/v1").MapMultiLocationApi();

app.Run();