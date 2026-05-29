using System.ComponentModel;
using Forecast.Controllers;
using Forecast.Models.Weather;
using Forecast.Services;
using Forecast.Shared.Responses;
using Forecast.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forecast.Api;

public static class ForecastApi
{
    public static RouteGroupBuilder MapForecastApi(this RouteGroupBuilder groups)
    {
        groups
            .MapGet("forecast", HandleGetForecast)
            .WithName("GetForecast")
            .WithDisplayName("Get Weather Forecast")
            .WithTags(["forecast"])
            .WithDescription(
                "Returns weather forecast for given coordinates or city. " +
                "Available providers: openweather, googleweather. " +
                "Default forecast: 7 days");

        return groups;
    }

    private static async Task<
        Results<Ok<Success<WeatherForecast>>, BadRequest<Status>,
               InternalServerError<Status>>
    > HandleGetForecast(
        [FromServices] ForecastController controller,
        [FromServices] CityCoordinatesService cityService,
        string? city,
        string? lat,
        string? lon,
        [DefaultValue("7")] int days,
        [DefaultValue("openweather")] string provider
    )
    {
        try
        {
            decimal latitude;
            decimal longitude;

            if (!string.IsNullOrEmpty(city))
            {
                (latitude, longitude) = cityService.GetCoordinates(city);
            }
            else if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lon))
            {
                latitude = decimal.Parse(lat);
                longitude = decimal.Parse(lon);
            }
            else
            {
                return TypedResults.BadRequest(
                    Status.Create(400, "provide either city or coordinates"));
            }

            var forecast = await controller.GetForecast(
                latitude, longitude, days, provider);

            return TypedResults.Ok(Success.Create(200, "success", forecast));
        }
        catch (FormatException)
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