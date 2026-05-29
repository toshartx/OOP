using Xunit;
using Forecast.Shared.Responses;
using System.Net;

namespace Tests;

public class ResponseTests
{
    [Fact]
    public void Status_Create_ReturnsCorrectCode()
    {
        var status = Status.Create(HttpStatusCode.BadRequest, "error");
        Assert.Equal(400, status.Code);
        Assert.Equal("error", status.Message);
    }

    [Fact]
    public void Success_Create_ReturnsCorrectData()
    {
        var success = Success.Create(200, "ok", "data");
        Assert.Equal(200, success.Code);
        Assert.Equal("ok", success.Message);
        Assert.Equal("data", success.Data);
    }
}