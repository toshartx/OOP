# Weather Forecast Example Project

## Docs
[swagger](http://localhost:8080/swagger/index.html)

The documentation is generated each time the app is started and
is stored only in memory. If one desires to obtain a schema json file, they could
download it via swagger web ui. Alternatively, api description can be generated manually with the following command:
```bash
nswag aspnetcore2openapi /project:Forecast.csproj /documentName:WeatherExampleAPI /output:docs/swagger.json
```
That, however, requires having [NSwag CLI](https://github.com/RicoSuter/NSwag/wiki/CommandLine) tool installed.

## Base commands
- run: `dotnet run`

## Environment variables
Pass API key and OpenWeather base url via environment variables:
```
OPENWEATHER_BASE_URL=https://api.openweathermap.org/data/2.5
OPENWEATHER_API_KEY=1fedeefce5fc92e841e033b617e8e590
GOOGLE_WEATHER_BASE_URL=https://weather.googleapis.com/v1
GOOGLE_WEATHER_API_KEY=AIzaSyBy2dpXOFBXAxw0BBDqHYoIrsxUDDjixc0
```

For convenience, the program will also read these keys from a `.env`
file, if one is placed in the root directory of the project.
