using Forecast.Clients;

namespace Forecast.Services;

public interface IWeatherProviderFactory
{
    IWeatherDataClient GetProvider(string providerName);
}