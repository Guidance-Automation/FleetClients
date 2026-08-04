using Microsoft.Extensions.Logging;

namespace Guidance.FleetClients;

internal static class SubscriptionCallbackDispatcher
{
    public static void Invoke<T>(
        Action<T>? callbacks,
        T update,
        ILogger? logger,
        string clientName)
    {
        if (callbacks == null)
            return;

        foreach (Action<T> callback in callbacks.GetInvocationList().Cast<Action<T>>())
        {
            try
            {
                callback(update);
            }
            catch (Exception ex)
            {
                logger?.LogWarningIfEnabled(
                    ex,
                    $"[{clientName}] Subscription callback failed");
            }
        }
    }
}
