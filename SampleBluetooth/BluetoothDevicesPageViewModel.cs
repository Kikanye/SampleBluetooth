using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using SampleBluetooth.DependencyServices;
using Xamarin.Forms;

namespace SampleBluetooth
{
    public class BluetoothDevicesPageViewModel : ViewModelBase
    {
        private IBluetoothLE _ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter _adapter;
        private CancellationTokenSource _bluetoothWorkCancellationTokenSource;

        #region commands
        public ICommand AddDeviceCommand { get; private set; }
        #endregion
        

        public BluetoothDevicesPageViewModel()
        {
            AddDeviceCommand = new Command(AddDeviceButtonClicked);
            _bluetoothWorkCancellationTokenSource = new CancellationTokenSource();
        }
        
        private async Task<bool> HasAndRequestBluetoothPermissionAsync()
        {
            var hasBluetoothPermission = await CustomDependencyServices.AppHasBluetoothPermission();
            if (!hasBluetoothPermission)
            {
                await CustomDependencyServices.RequestBluetoothPermission();
                hasBluetoothPermission = await CustomDependencyServices.AppHasBluetoothPermission();
            }
            return hasBluetoothPermission;
        }

        private async void AddDeviceButtonClicked()
        {
            this._ble = CrossBluetoothLE.Current;
            this._adapter = _ble.Adapter;
            var res = await HasAndRequestBluetoothPermissionAsync();
            Console.WriteLine(res);
        }

    }
}
