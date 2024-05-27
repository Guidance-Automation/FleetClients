using GAAPICommon.Services.FleetManager;
using Microsoft.Extensions.DependencyInjection;

namespace GAClients.FleetClients;

public static class DIExtensions
{
    /// <summary>
    /// Registers gRPC clients and their associated wrappers into the provided IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to which the gRPC clients and their associated wrappers will be added.</param>
    /// <param name="endpoint">The base URI for the gRPC services. All registered gRPC clients will use this URI to connect to the server.</param>
    /// <returns>The IServiceCollection with clients added.</returns>
    public static IServiceCollection RegisterFleetManagerClient(this IServiceCollection services, Uri endpoint, bool subscribe)
    {
        services.AddSingleton(f => new FleetManagerClientSettings(subscribe));
        services.AddGrpcClientExt<FleetManagerServiceProto.FleetManagerServiceProtoClient>(endpoint);
        services.AddTransient<IFleetManagerClient, FleetManagerClient>();
        return services;
    }

    private static IServiceCollection AddGrpcClientExt<T>(this IServiceCollection services, Uri uri) where T : class
    {
        services.AddGrpcClient<T>(o =>
        {
            o.Address = uri;
        });
        return services;
    }
}