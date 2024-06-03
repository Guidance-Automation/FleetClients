namespace GAClients.FleetClients;

/// <summary>
/// Settings to pass to the Fleet Manager Client.
/// </summary>
/// <param name="Subscribe"> If true, spin up a new thread to subscribe and receive fleet updates. </param>
/// <param name="RethrowExceptions"> If true, rethrow all exceptions to handle on the consumer side. </param>
public record FleetManagerClientSettings(bool Subscribe, bool RethrowExceptions);
