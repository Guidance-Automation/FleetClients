using System.Net;

namespace GAClients.FleetClients.DevWPFApp;

internal static class Bootstrapper
{
    public static void Activate()
    {
        IFleetManagerClient client = ClientFactory.CreateFleetManagerClient(IPAddress.Loopback);
        ViewModel.ViewModelLocator.FleetManagerClientViewModel.Model = client;
    }
}
