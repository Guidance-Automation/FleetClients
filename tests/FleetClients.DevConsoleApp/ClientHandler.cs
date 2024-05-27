using GAAPICommon.Constructors;
using System.Net;

namespace GAClients.FleetClients.DevConsoleApp;

public class ClientHandler
{
    IFleetManagerClient? _client;

    public void Init()
    {
        _client = ClientFactory.CreateFleetManagerClient(IPAddress.Loopback);
        _client.FleetStateUpdated += FleetManager_ServiceRequest;
    }

    private void FleetManager_ServiceRequest(FleetState fleetStateData)
    {
        string strLine;

        if (fleetStateData.Tick < 10)
        {
            strLine = string.Format($"Ticks =   {fleetStateData.Tick},  Vehicles = {fleetStateData.KingpinStates?.Length}");
        }
        else if (fleetStateData.Tick < 100)
        {
            strLine = string.Format($"Ticks =  {fleetStateData.Tick},  Vehicles = {fleetStateData.KingpinStates?.Length}");
        }
        else
        {
            strLine = string.Format($"Ticks = {fleetStateData.Tick},  Vehicles = {fleetStateData.KingpinStates?.Length}");
        }
        Console.WriteLine(strLine);

    }
}