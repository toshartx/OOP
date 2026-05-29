using DotEnv.Core;
using Forecast.Api;
using Forecast.Clients;
using Forecast.Controllers;
using Forecast.Services;

var builder = WebApplication.CreateBuilder(args);

// Loading configuration as environment variables from the .env file.
new EnvLoader().Load();
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "WeatherExampleAPI";
    config.Title = "Weather Example API";
    config.Version = "v1";
});

// Регистрация HTTP клиентов для провайдеров погоды
builder.Services.AddHttpClient<OpenWeatherDataClient>();
builder.Services.AddHttpClient<GoogleWeatherDataClient>();
// Регистрация сервиса городов
builder.Services.AddSingleton<CityCoordinatesService>();
// Регистрация клиентов как IWeatherDataClient
builder.Services.AddSingleton<IWeatherDataClient, OpenWeatherDataClient>(
    sp => sp.GetRequiredService<OpenWeatherDataClient>());
builder.Services.AddSingleton<IWeatherDataClient, GoogleWeatherDataClient>(
    sp => sp.GetRequiredService<GoogleWeatherDataClient>());

// Регистрация фабрики
builder.Services.AddSingleton<IWeatherProviderFactory, WeatherProviderFactory>();

// Регистрация контроллера
builder.Services.AddSingleton<CurrentWeatherController>();
builder.Services.AddSingleton<ForecastController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseDeveloperExceptionPage();
}

app.MapGroup("/api/v1").MapCurrentWeatherApi();
app.MapGroup("/api/v1").MapForecastApi();

app.Run();