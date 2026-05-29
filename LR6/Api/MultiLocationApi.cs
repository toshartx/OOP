using System.ComponentModel;
using Forecast.Controllers;
using Forecast.Models.Weather;
using Forecast.Shared.Responses;
using Forecast.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forecast.Api;

public static class MultiLocationApi
{
    public static RouteGroupBuilder MapMultiLocationApi(this RouteGroupBuilder groups)
    {
        groups
            .MapGet("multi-weather", HandleGetMultiLocation)
            .WithName("GetMultiLocationWeather")
            .WithDisplayName("Get Multi-Location Weather")
            .WithTags(["weather"])
            .WithDescription(
                "Returns current weather for multiple locations. " +
                "Locations can be city names or coordinates (lat,lon). " +
                "Example: ?locations=Минск&locations=51.5,-0.1&locations=Токио");

        return groups;
    }

    private static async Task<
        Results<Ok<Success<List<LocationTemperature>>>, BadRequest<Status>,
               InternalServerError<Status>>
    > HandleGetMultiLocation(
        [FromServices] MultiLocationController controller,
        [FromQuery] string[] locations,
        [DefaultValue("openweather")] string provider
    )
    {
        try
        {
            if (locations == null || locations.Length == 0)
            {
                return TypedResults.BadRequest(
                    Status.Create(400, "at least one location required"));
            }

            var results = await controller.GetMultiLocationTemperature(
                locations.ToList(), provider);
            return TypedResults.Ok(Success.Create(200, "success", results));
        }
        catch (FormatException)
        {
            return TypedResults.BadRequest(Status.Create(400, "invalid coordinates format"));
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