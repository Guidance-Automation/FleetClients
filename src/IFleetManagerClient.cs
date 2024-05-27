using GAAPICommon.Constructors;
using GAAPICommon.Enums;
using GAAPICommon.Messages;
using System.Net;

namespace GAClients.FleetClients;

/// <summary>
/// Client for interacting with the FleetManager service.
/// </summary>
public interface IFleetManagerClient : IDisposable
{
    /// <summary>
    /// Event that is triggered when the fleet state is updated.
    /// </summary>
    public event Action<FleetState>? FleetStateUpdated;

    /// <summary>
    /// Creates a virtual vehicle.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The initial pose of the vehicle.</param>
    /// <returns>True if the vehicle was created successfully, otherwise false.</returns>
    public bool CreateVirtualVehicle(IPAddress ipAddress, PoseDto pose);

    /// <summary>
    /// Creates a virtual vehicle asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The initial pose of the vehicle.</param>
    /// <returns>True if the vehicle was created successfully, otherwise false.</returns>
    public Task<bool> CreateVirtualVehicleAsync(IPAddress ipAddress, PoseDto pose);

    /// <summary>
    /// Gets the kingpin description.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the kingpin description.</returns>
    public string? GetKingpinDescription(string ipAddress);

    /// <summary>
    /// Gets the kingpin description asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the kingpin description.</returns>
    public Task<string?> GetKingpinDescriptionAsync(string ipAddress);

    /// <summary>
    /// Removes a vehicle.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle to remove.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool RemoveVehicle(string ipAddress);

    /// <summary>
    /// Removes a vehicle asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle to remove.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public Task<bool> RemoveVehicleAsync(string ipAddress);

    /// <summary>
    /// Sets the fleet state.
    /// </summary>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool SetFleetState(VehicleControllerState controllerState);

    /// <summary>
    /// Sets the fleet state asynchronously.
    /// </summary>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public Task<bool> SetFleetStateAsync(VehicleControllerState controllerState);

    /// <summary>
    /// Sets the frozen state of the fleet.
    /// </summary>
    /// <param name="frozenState">The frozen state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generic result of the operation.</returns>
    public bool SetFrozenState(FrozenState frozenState);

    /// <summary>
    /// Sets the frozen state of the fleet asynchronously.
    /// </summary>
    /// <param name="frozenState">The frozen state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generic result of the operation.</returns>
    public Task<bool> SetFrozenStateAsync(FrozenState frozenState);

    /// <summary>
    /// Sets the kingpin state.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool SetKingpinState(string ipAddress, VehicleControllerState controllerState);

    /// <summary>
    /// Sets the kingpin state asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public Task<bool> SetKingpinStateAsync(string ipAddress, VehicleControllerState controllerState);

    /// <summary>
    /// Sets the pose of a vehicle.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The new pose of the vehicle.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool SetPose(string ipAddress, PoseDto pose);

    /// <summary>
    /// Sets the pose of a vehicle asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The new pose of the vehicle.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public Task<bool> SetPoseAsync(string ipAddress, PoseDto pose);
}