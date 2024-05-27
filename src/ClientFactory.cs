using Microsoft.Extensions.Logging;
using System.Net;

namespace GAClients.FleetClients;

public static class ClientFactory
{
    public static IFleetManagerClient CreateFleetManagerClient(IPAddress ipAddress, ushort httpPort = 41917, bool subscribe = true, ILogger<FleetManagerClient>? logger = null)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        Uri uri = new($"http://{ipAddress}:{httpPort}");
        FleetManagerClientSettings settings = new(subscribe);
        return new FleetManagerClient(uri, settings, logger);
    }
}