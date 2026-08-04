using GAAPICommon;
using GAAPICommon.Constructors;
using GAAPICommon.Enums;
using GAAPICommon.Messages;
using GAAPICommon.Services.FleetManager;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Guidance.FleetClients;

/// <summary>
/// Client for interacting with the FleetManager service.
/// </summary>
public class FleetManagerClient : IFleetManagerClient
{
    private bool _isDisposed;
    private readonly CancellationTokenSource _cts = new();
    private readonly FleetManagerServiceProto.FleetManagerServiceProtoClient _client;
    private readonly ILogger? _logger;
    private FleetState? _fleetState;
    private readonly bool _rethrow;

    /// <summary>
    /// Event that is triggered when the fleet state is updated.
    /// </summary>
    public event Action<FleetState>? FleetStateUpdated;

    /// <summary>
    /// Initializes a new instance of the FleetManagerClient class using an existing client instance.
    /// Intended for use with dependency injection.
    /// </summary>
    /// <param name="client">An existing instance of the FleetManagerServiceProtoClient.</param>
    /// <param name="settings">The settings for the client.</param>
    /// <param name="logger">Logger for logging messages.</param>
    public FleetManagerClient(FleetManagerServiceProto.FleetManagerServiceProtoClient client, FleetManagerClientSettings settings, ILogger<FleetManagerClient>? logger)
    {
        _client = client;
        _logger = logger;
        _logger?.LogInformationIfEnabled("[FleetManagerClient] FleetManagerClient created with existing client instance");
        _rethrow = settings.RethrowExceptions;
        if (settings.Subscribe)
            Task.Run(Subscribe);
    }

    /// <summary>
    /// Get the latest fleet state.
    /// </summary>
    /// <returns>The most recent fleet state.</returns>
    public FleetState? GetFleetState()
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] GetFleetState() called");
        return _fleetState;
    }

    /// <summary>
    /// Creates a virtual vehicle.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The initial pose of the vehicle.</param>
    /// <returns>True if the vehicle was created successfully, otherwise false.</returns>
    public IPAddress? CreateVirtualVehicle(PoseDto pose)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] CreateVirtualVehicle() called");
        try
        {
            CreateVirtualVehicleRequest request = new()
            {
                Pose = pose
            };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending CreateVirtualVehicleRequest");
            CreateVirtualVehicleResult response = _client.CreateVirtualVehicle(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("CreateVirtualVehicle() succeeded");
                return IPAddress.Parse(response.IPAddress);
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] Sending CreateVirtualVehicle() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error creating virtual vehicle");
            if (_rethrow)
                throw;
            return null;
        }
    }

    /// <summary>
    /// Creates a virtual vehicle asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The initial pose of the vehicle.</param>
    /// <returns>True if the vehicle was created successfully, otherwise false.</returns>
    public async Task<IPAddress?> CreateVirtualVehicleAsync(PoseDto pose)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] CreateVirtualVehicleAsync() called");
        try
        {
            CreateVirtualVehicleRequest request = new()
            {
                Pose = pose
            };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending CreateVirtualVehicleRequest");
            CreateVirtualVehicleResult response = await _client.CreateVirtualVehicleAsync(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("CreateVirtualVehicleAsync() succeeded");
                return IPAddress.Parse(response.IPAddress);
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] Sending CreateVirtualVehicleAsync() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error creating virtual vehicle");
            if (_rethrow)
                throw;
            return null;
        }
    }

    /// <summary>
    /// Gets the kingpin description.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the kingpin description.</returns>
    public string? GetKingpinDescription(string ipAddress)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] GetKingpinDescription() called with {IpAddress}", ipAddress);
        try
        {
            GetKingpinDescriptionRequest request = new() { IPAddress = ipAddress };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending GetKingpinDescriptionRequest");
            GetKingpinDescriptionResult response = _client.GetKingpinDescription(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] GetKingpinDescription() succeeded");
                return response.KingpinDescription;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] GetKingpinDescription() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error getting kingpin description");
            if (_rethrow)
                throw;
            return null;
        }
    }

    /// <summary>
    /// Gets the kingpin description asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the kingpin description.</returns>
    public async Task<string?> GetKingpinDescriptionAsync(string ipAddress)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] GetKingpinDescriptionAsync() called with {IpAddress}", ipAddress);
        try
        {
            GetKingpinDescriptionRequest request = new() { IPAddress = ipAddress };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending GetKingpinDescriptionRequest");
            GetKingpinDescriptionResult response = await _client.GetKingpinDescriptionAsync(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] GetKingpinDescriptionAsync() succeeded");
                return response.KingpinDescription;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] GetKingpinDescriptionAsync() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error getting kingpin description");
            if (_rethrow)
                throw;
            return null;
        }
    }

    /// <summary>
    /// Removes a vehicle.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle to remove.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool RemoveVehicle(string ipAddress)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] RemoveVehicle() called with {IpAddress}", ipAddress);
        try
        {
            RemoveVehicleRequest request = new() { IPAddress = ipAddress };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending RemoveVehicleRequest");
            GenericResult response = _client.RemoveVehicle(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] RemoveVehicle() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] RemoveVehicle() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error removing vehicle");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Removes a vehicle asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle to remove.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public async Task<bool> RemoveVehicleAsync(string ipAddress)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] RemoveVehicleAsync() called with {IpAddress}", ipAddress);
        try
        {
            RemoveVehicleRequest request = new() { IPAddress = ipAddress };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending RemoveVehicleRequest");
            GenericResult response = await _client.RemoveVehicleAsync(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] RemoveVehicleAsync() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] RemoveVehicleAsync() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error removing vehicle");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the fleet state.
    /// </summary>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool SetFleetState(VehicleControllerState controllerState)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetFleetState() called with {ControllerState}", controllerState);
        try
        {
            SetFleetStateRequest request = new() { ControllerState = controllerState };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetFleetStateRequest");
            GenericResult response = _client.SetFleetState(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetFleetState() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] SetFleetState() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting fleet state");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the fleet state asynchronously.
    /// </summary>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public async Task<bool> SetFleetStateAsync(VehicleControllerState controllerState)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetFleetStateAsync() called with {ControllerState}", controllerState);
        try
        {
            SetFleetStateRequest request = new() { ControllerState = controllerState };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetFleetStateRequest");
            GenericResult response = await _client.SetFleetStateAsync(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetFleetStateAsync() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] SetFleetStateAsync() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting fleet state");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the frozen state of the fleet.
    /// </summary>
    /// <param name="frozenState">The frozen state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generic result of the operation.</returns>
    public bool SetFrozenState(FrozenState frozenState)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetFrozenState() called with {FrozenState}", frozenState);
        try
        {
            SetFrozenStateRequest request = new() { FrozenState = frozenState };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetFrozenStateRequest");
            GenericResult response = _client.SetFrozenState(request);
            _logger?.LogInformationIfEnabled("[FleetManagerClient] SetFrozenState() succeeded");
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetFrozenState() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] Sending SetFrozenState() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting fleet frozen state");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the frozen state of the fleet asynchronously.
    /// </summary>
    /// <param name="frozenState">The frozen state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generic result of the operation.</returns>
    public async Task<bool> SetFrozenStateAsync(FrozenState frozenState)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetFrozenStateAsync() called with {FrozenState}", frozenState);
        try
        {
            SetFrozenStateRequest request = new() { FrozenState = frozenState };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetFrozenStateRequest");
            GenericResult response = await _client.SetFrozenStateAsync(request);
            _logger?.LogInformationIfEnabled("[FleetManagerClient] SetFrozenStateAsync() succeeded");
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetFrozenStateAsync() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] Sending SetFrozenStateAsync() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting fleet frozen state");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the kingpin state.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool SetKingpinState(string ipAddress, VehicleControllerState controllerState)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetKingpinState() called with {IpAddress} and {ControllerState}", ipAddress, controllerState);
        try
        {
            SetKingpinStateRequest request = new() { IPAddress = ipAddress, ControllerState = controllerState };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetKingpinState");
            GenericResult response = _client.SetKingpinState(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetKingpinState() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] SetKingpinState() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting kingpin state");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the kingpin state asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the kingpin.</param>
    /// <param name="controllerState">The controller state to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public async Task<bool> SetKingpinStateAsync(string ipAddress, VehicleControllerState controllerState)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetKingpinStateAsync() called with {IpAddress} and {ControllerState}", ipAddress, controllerState);
        try
        {
            SetKingpinStateRequest request = new() { IPAddress = ipAddress, ControllerState = controllerState };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetKingpinStateRequest");
            GenericResult response = await _client.SetKingpinStateAsync(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetKingpinStateAsync() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] SetKingpinStateAsync() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting kingpin state");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the pose of a vehicle.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The new pose of the vehicle.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public bool SetPose(string ipAddress, PoseDto pose)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetPose() called with {IpAddress} and {Pose}", ipAddress, pose);
        try
        {
            SetPoseRequest request = new() { IPAddress = ipAddress, Pose = pose };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetPoseRequest");
            GenericResult response = _client.SetPose(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetPose() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] SetPose() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting pose");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Sets the pose of a vehicle asynchronously.
    /// </summary>
    /// <param name="ipAddress">The IP address of the vehicle.</param>
    /// <param name="pose">The new pose of the vehicle.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the success status of the operation.</returns>
    public async Task<bool> SetPoseAsync(string ipAddress, PoseDto pose)
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] SetPoseAsync() called with {IpAddress} and {Pose}", ipAddress, pose);
        try
        {
            SetPoseRequest request = new() { IPAddress = ipAddress, Pose = pose };
            _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SetPoseRequest");
            GenericResult response = await _client.SetPoseAsync(request);
            if (response.ServiceCode == (int)ServiceCode.NoError)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] SetPoseAsync() succeeded");
                return true;
            }
            else
            {
                _logger?.LogErrorIfEnabled("[FleetManagerClient] SetPoseAsync() failed with {ServiceCode} and message {ExceptionMessage}", response.ServiceCode, response.ExceptionMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogErrorIfEnabled(ex, "[FleetManagerClient] Error setting pose");
            if (_rethrow)
                throw;
            return false;
        }
    }

    /// <summary>
    /// Unsubscribe from fleet state updates.
    /// </summary>
    public void Unsubscribe()
    {
        _logger?.LogInformationIfEnabled("[FleetManagerClient] unsubscribing from fleet state updates");
        _cts.Cancel();
    }

    /// <summary>
    /// Subscribes to fleet state updates and raises the FleetStateUpdated event when an update is received.
    /// </summary>
    private async Task Subscribe()
    {
        _logger?.LogTraceIfEnabled("[FleetManagerClient] Subscribe() started");
        while (!_cts.IsCancellationRequested)
        {
            try
            {
                SubscribeRequest subscribeRequest = new();
                _logger?.LogDebugIfEnabled("[FleetManagerClient] Sending SubscribeRequest");
                using AsyncServerStreamingCall<FleetStateDto> streamingCall = _client.Subscribe(subscribeRequest);

                await foreach (FleetStateDto? fleetStateDto in streamingCall.ResponseStream.ReadAllAsync(_cts.Token))
                {
                    _logger?.LogTraceIfEnabled("[FleetManagerClient] Received FleetStateDto: {FleetStateDto}", fleetStateDto);
                    _fleetState = fleetStateDto.ToFleetState();
                    SubscriptionCallbackDispatcher.Invoke(
                        FleetStateUpdated,
                        _fleetState,
                        _logger,
                        nameof(FleetManagerClient));
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                _logger?.LogInformationIfEnabled("[FleetManagerClient] Subscription cancelled");
                break;
            }
            catch (OperationCanceledException) when (_cts.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger?.LogWarningIfEnabled(ex, "[FleetManagerClient] Exception during subscription. Retrying...");
                try
                {
                    await Task.Delay(100, _cts.Token);
                }
                catch (OperationCanceledException) when (_cts.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        _logger?.LogTraceIfEnabled("[FleetManagerClient] Subscribe() ended");
    }

    /// <summary>
    /// Disposes of the client resources.
    /// </summary>
    /// <param name="disposing">Indicates whether the method is called from the Dispose method or from a finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            _logger?.LogTraceIfEnabled("[FleetManagerClient] Disposing resources");
            Unsubscribe();
            _cts.Dispose();
        }

        _isDisposed = true;
        _logger?.LogInformationIfEnabled("[FleetManagerClient] FleetManagerClient disposed");
    }

    /// <summary>
    /// Disposes of the client, releasing all managed resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
