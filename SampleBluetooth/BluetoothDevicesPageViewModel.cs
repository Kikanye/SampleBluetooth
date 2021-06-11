using System.Threading;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace SampleBluetooth
{
    public class BluetoothDevicesPageViewModel : ViewModelBase
    {
        private IPageNavService _pageService;
        private CancellationTokenSource _bluetoothWorkCancellationTokenSource;

        #region commands
        public ICommand AddDeviceCommand { get; private set; }
        #endregion
        

        public BluetoothDevicesPageViewModel(IPageNavService pageNavService)
        {
            AddDeviceCommand = new Command(AddDeviceButtonClicked);
            _bluetoothWorkCancellationTokenSource = new CancellationTokenSource();
        }

        private async void AddDeviceButtonClicked()
        {
            await _pageService.PushAsync(PopupNavigation.Instance, new AddBluetoothDevicePage());
        }

    }
}
