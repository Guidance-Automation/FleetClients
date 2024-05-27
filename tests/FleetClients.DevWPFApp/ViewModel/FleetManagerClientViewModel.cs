using GAAPICommon.Constructors;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GACore.UI.Command;
using GAAPICommon.Enums;

namespace GAClients.FleetClients.DevWPFApp.ViewModel;

public class FleetManagerClientViewModel : INotifyPropertyChanged
{
    private FleetState? _fleetState = null;
    private IFleetManagerClient? _model = null;

    public event PropertyChangedEventHandler? PropertyChanged;

    public FleetManagerClientViewModel()
    {
        FreezeCommand = new CustomCommand(Freeze, t => true);
        UnfreezeCommand = new CustomCommand(Unfreeze, t => true);   
    }

    private void HandleModelUpdate()
    {
        if (_model != null)
            _model.FleetStateUpdated += Model_FleetStateUpdated;
    }

    public FleetState? FleetState
    {
        get { return _fleetState; }
        private set
        {
            _fleetState = value;
            OnNotifyPropertyChanged();
        }
    }

    public ICommand FreezeCommand { get; set; }

    public ICommand UnfreezeCommand { get; set; }

    private void Unfreeze(object? obj)
    {
        ViewModelLocator.FleetManagerClientViewModel.Model?.SetFrozenStateAsync(FrozenState.Unfrozen);
    }

    private void Freeze(object? obj)
    {
        ViewModelLocator.FleetManagerClientViewModel.Model?.SetFrozenStateAsync(FrozenState.Frozen);
    }

    protected void OnNotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        Task.Run(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
    }

    private void Model_FleetStateUpdated(FleetState fleetState)
    {
        FleetState = fleetState;
    }

    public IFleetManagerClient? Model
    {
        get { return _model; }
        set
        {
            _model = value;
            HandleModelUpdate();
        }
    }
}