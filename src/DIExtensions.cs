using GAAPICommon.Services.FleetManager;
using Microsoft.Extensions.DependencyInjection;

namespace GAClients.FleetClients;

/// <summary>
/// Extension methods to provide support for dependancy injection.
/// </summary>
public static class DIExtensions
{
    /// <summary>
    /// Registers gRPC clients and their associated wrappers into the provided IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to which the gRPC clients and their associated wrappers will be added.</param>
    /// <param name="endpoint">The base URI for the gRPC services. All registered gRPC clients will use this URI to connect to the server.</param>
    /// <param name="subscribe">Automatically subscribe when instantiating the client.</param>
    /// <param name="rethrowExceptions">When true, caught exceptions will be rethrown to allow them to be captured by the package consumer.</param>
    /// <returns>The IServiceCollection with clients added.</returns>
    public static IServiceCollection RegisterFleetManagerClient(this IServiceCollection services, Uri endpoint, bool subscribe, bool rethrowExceptions)
    {
        services.AddSingleton(f => new FleetManagerClientSettings(subscribe, rethrowExceptions));
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