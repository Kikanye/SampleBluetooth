using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleBluetooth.DependencyServices
{

    public static class CustomDependencyServices
    {

        #region Bluetooth

        public static async Task<bool> AppHasBluetoothPermission()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                return await Task.FromResult(DependencyService.Get<IBluetoothPermissionService>()
                    .HasBluetoothPermission());
            }
            return false;
        }

        public static async Task RequestBluetoothPermission()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                await Task.Run(() =>
                {
                    DependencyService.Get<IBluetoothPermissionService>().RequestBluetoothPermission();
                });
            }
        }

        #endregion
    }

    public interface IBluetoothPermissionService
    {
        bool HasBluetoothPermission();
        void RequestBluetoothPermission();
    }
}