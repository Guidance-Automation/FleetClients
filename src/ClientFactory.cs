using GAAPICommon.Services.FleetManager;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GAClients.FleetClients;

/// <summary>
/// Class containing methods to create the client without requiring the use of DI.
/// </summary>
public static class ClientFactory
{
    /// <summary>
    /// Create a new instance of the <see cref="FleetManagerClient"/>
    /// </summary>
    /// <param name="ipAddress">The IP Address of the server to connect to.</param>
    /// <param name="httpPort">The port of the server to connect to.</param>
    /// <param name="subscribe">Automatically subscribe when instantiating the client.</param>
    /// <param name="rethrowExceptions">When true, caught exceptions will be rethrown to allow them to be captured by the package consumer.</param>
    /// <param name="logger">The instance of a logger to use.</param>
    /// <returns>A new <see cref="FleetManagerClient"/></returns>
    public static IFleetManagerClient CreateFleetManagerClient(IPAddress ipAddress, ushort httpPort = 41917, bool subscribe = true, bool rethrowExceptions = false, ILogger<FleetManagerClient>? logger = null)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        Uri uri = new($"http://{ipAddress}:{httpPort}");
        FleetManagerClientSettings settings = new(subscribe, rethrowExceptions);
        GrpcChannel channel = GrpcChannel.ForAddress(uri);
        FleetManagerServiceProto.FleetManagerServiceProtoClient client = new(channel);
        return new FleetManagerClient(client, settings, logger);
    }
}