using CoreBluetooth;
using CoreFoundation;
using SampleBluetooth.DependencyServices;
using SampleBluetooth.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BluetoothPermissionService))]
namespace SampleBluetooth.iOS
{
    public class BluetoothPermissionService : IBluetoothPermissionService
    {
        public bool HasBluetoothPermission()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                return CBCentralManager.Authorization == CBManagerAuthorization.AllowedAlways;
            }
            return true;
        }

        public void RequestBluetoothPermission()
        {
            //TODO: This currently does not work... make it work at some point
            var myDelegate = new PermissionCBCentralManager(this);
            var centralManger = new CBCentralManager(myDelegate, DispatchQueue.MainQueue,
                new CBCentralInitOptions() { ShowPowerAlert = true });
        }
        
        internal void CurrentUpdatedState(CBCentralManager central)
        {
            //what to do here?
        }

    }

    public class PermissionCBCentralManager : CBCentralManagerDelegate
    {
        readonly BluetoothPermissionService _permissionService = null;

        public PermissionCBCentralManager(BluetoothPermissionService controller)
        {
            _permissionService = controller;
        }
        
        public override void UpdatedState(CBCentralManager central)
        {
            _permissionService.CurrentUpdatedState(central);
        }
    }
    
}