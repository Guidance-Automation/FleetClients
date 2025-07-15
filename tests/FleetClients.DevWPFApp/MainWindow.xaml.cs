using System.Windows;

namespace Guidance.FleetClients.DevWPFApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Bootstrapper.Activate();
    }
}
