using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Forecast.Controllers;
using Forecast.Models.Weather;
using Forecast.Services;
using Forecast.Shared.Responses;
using Forecast.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forecast.Api;
[ExcludeFromCodeCoverage]
public static class WeatherApi
{
    public static RouteGroupBuilder MapCurrentWeatherApi(this RouteGroupBuilder groups)
    {
        groups
            .MapGet("weather", HandleGetCurrentWeather)
            .WithName("GetCurrentWeather")
            .WithDisplayName("Get Current Weather")
            .WithTags(["weather"])
            .WithDescription(
                "Returns current weather for given coordinates or city name. " +
                "Available providers: openweather, googleweather. " +
                "Available cities: Минск, Лондон, Токио, Шанхай, Варшава");

        return groups;
    }

    private static async Task<
        Results<Ok<Success<CurrentWeather>>, BadRequest<Status>,
               InternalServerError<Status>>
    > HandleGetCurrentWeather(
        [FromServices] CurrentWeatherController controller,
        [FromServices] CityCoordinatesService cityService,
        [DefaultValue("18.300231990440125")] string? lat,
        [DefaultValue("-64.8251590359234")] string? lon,
        [DefaultValue("openweather")] string provider,
        string? city
    )
    {
        try
        {
            decimal latitude;
            decimal longitude;

            // Если передан город — получаем координаты из сервиса
            if (!string.IsNullOrEmpty(city))
            {
                (latitude, longitude) = cityService.GetCoordinates(city);
            }
            else if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lon))
            {
                latitude = decimal.Parse(lat, CultureInfo.InvariantCulture);
                longitude = decimal.Parse(lon, CultureInfo.InvariantCulture);
            }
            else
            {
                return TypedResults.BadRequest(
                    Status.Create(400, "provide either city or coordinates"));
            }

            var weather = await controller.GetCurrentWeather(
                latitude, longitude, provider);

            return TypedResults.Ok(Success.Create(200, "success", weather));
        }
        catch (FormatException)
        {
            return TypedResults.BadRequest(Status.Create(400, "invalid coordinates"));
        }
        catch (OverflowException)
        {
            return TypedResults.BadRequest(Status.Create(400, "invalid coordinates"));
        }
        catch (ArgumentException e)
        {
            return TypedResults.BadRequest(Status.Create(400, e.Message));
        }
        catch (ApiCallException e)
        {
            return TypedResults.InternalServerError(Status.Create(500, e.Message));
        }
    }
}