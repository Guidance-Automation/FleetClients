namespace Guidance.FleetClients.DevConsoleApp;

public class Program
{
    static void Main(string[] _)
    {
        ClientHandler handler = new();
        handler.Init();
    }
}
