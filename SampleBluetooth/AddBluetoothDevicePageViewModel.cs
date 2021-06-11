using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Rg.Plugins.Popup.Services;
using SampleBluetooth.DependencyServices;
using Xamarin.Forms;

namespace SampleBluetooth
{
public class AddBluetoothDevicePageViewModel: ViewModelBase
    {
        private IPageNavService _pageService;
        
        private IBluetoothLE _ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter _adapter;

        private ICharacteristic sendCharacteristic;
        private ICharacteristic receiveCharacteristic;

        private CancellationTokenSource _scannerCancellationTokenSource;
        private CancellationTokenSource _connectionCancellationTokenSource;
        private CancellationTokenSource _getServicesCancallationTokenSource;

        private bool _shouldClosePopup;
        public ICommand ScanForDevicesCommand { get; set; }
        public ICommand CloseCommand { get; private set; }

        private bool _isConnecting;
        public bool IsConnecting
        {
            get { return _isConnecting; }
            set
            {
                if (_isConnecting != value)
                {
                    _isConnecting = value;
                    ActivityIndicatorOn = value;
                    //TODO: Localize.
                    ActivityText = "Connecting...";
                    OnPropertyChanged();
                }
            }
        }

        private bool _activityIndicatorOn;
        public bool ActivityIndicatorOn
        {
            get { return _activityIndicatorOn; }
            set
            {
                if (_activityIndicatorOn != value)
                {
                    _activityIndicatorOn = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private bool _isScanning;
        public bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                if (_isScanning != value)
                {
                    _isScanning = value;
                    ActivityIndicatorOn = value;
                    //TODO: Localize.
                    if (_isScanning)
                    {
                        ActivityText = "Scanning...";
                    }
                    OnPropertyChanged();
                }
            }
        }

        private bool _popupClosureAllowed;
        public bool PopupClosureAllowed
        {
            get { return _popupClosureAllowed; }
            set
            {
                if (_popupClosureAllowed != value)
                {
                    _popupClosureAllowed = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _activityText;
        public string ActivityText
        {
            get { return _activityText; }
            set
            {
                if (_activityText != value)
                {
                    _activityText = value;
                    OnPropertyChanged();
                }
            }
        }

        public AddBluetoothDevicePageViewModel(IPageNavService pageService)
        {
            this._ble = CrossBluetoothLE.Current;
            this._adapter = _ble.Adapter;

            this._adapter.DeviceDiscovered += DeviceDiscovered;

            _pageService = pageService;
            ScanForDevicesCommand = new Command(PerformScan);
            CloseCommand = new Command(Close);
            
            PopupClosureAllowed = true;
            
            _connectionCancellationTokenSource = new CancellationTokenSource();
            _scannerCancellationTokenSource = new CancellationTokenSource();
            _getServicesCancallationTokenSource = new CancellationTokenSource();
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

        public async void PerformScan()
        {
            if (IsScanning)
            {
                IsScanning = false;
                _scannerCancellationTokenSource.Cancel();
                return;
            }

            try
            {
                if (_ble.State == BluetoothState.On)
                {
                    IsScanning = true;
                    if (!await HasAndRequestBluetoothPermissionAsync())
                    {
                        await _pageService.DisplayAlert("Permission required",
                            "Application does not have Bluetooth permission.", "OK");
                        IsScanning = false;
                        return;
                    }

                    await _adapter.StartScanningForDevicesAsync(cancellationToken: _scannerCancellationTokenSource.Token);
                    _scannerCancellationTokenSource.Token.ThrowIfCancellationRequested();
                    PopupClosureAllowed = false;
                    IsScanning = false;
                    PopupClosureAllowed = true;
                }
                else if (_ble.State == BluetoothState.Unavailable || _ble.State == BluetoothState.Unauthorized)
                {
                    if (!await HasAndRequestBluetoothPermissionAsync())
                    {
                        await _pageService.DisplayAlert("Attention",
                            "Please confirm that app has bluetooth Permissions",
                            "OK");
                    }
                }
                else
                {
                    await _pageService.DisplayAlert("Attention",
                        "Bluetooth must be turned on to search for devices.",
                        "OK");
                }
            }

            catch (OperationCanceledException oex)
            {
                IsScanning = false;
                if (_shouldClosePopup)
                {
                    await PopupNavigation.Instance.PopAsync();
                    _shouldClosePopup = false;
                }
            }
        }

        private void DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if ((e.Device != null) && !String.IsNullOrEmpty(e.Device.Name))
            {
            }
        }
        
        private async void Close()
        {
            if (PopupClosureAllowed)
            {
                _connectionCancellationTokenSource.Cancel();
                _scannerCancellationTokenSource.Cancel();
                ActivityText = "Closing";
                if (!(IsScanning || IsConnecting))
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    _shouldClosePopup = true;
                }
            }
        }
    }
}