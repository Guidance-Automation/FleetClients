using GAAPICommon.Services.FleetManager;
using Grpc.Net.Client;
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
        GrpcChannel channel = GrpcChannel.ForAddress(uri);
        FleetManagerServiceProto.FleetManagerServiceProtoClient client = new(channel);
        return new FleetManagerClient(client, settings, logger);
    }
}