using System.Collections.Generic;
using Android;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using SampleBluetooth.DependencyServices;
using SampleBluetooth.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BluetoothPermissionService))]
namespace SampleBluetooth.Droid
{
    public class BluetoothPermissionService : IBluetoothPermissionService
    {
        public bool HasBluetoothPermission()
        {
            return HasAllBluetoothPermissions();
        }

        public void RequestBluetoothPermission()
        {
            var permissionsArray = GetMissingPermissions();
            if (permissionsArray != null && permissionsArray.Length > 0)
            {
                ActivityCompat.RequestPermissions(Platform.CurrentActivity, permissionsArray, 1);
            }
        }
        
        private bool HasAllBluetoothPermissions()
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                return HasAndroidBluetoothPermission() && HasAndroidBluetoothAdminPermission() && HasAndroidFineLocationPermission() &&
                       HasAndroidBackgroundLocationPermission();
            }
            if (Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.P)
            {
                return HasAndroidBluetoothPermission() && HasAndroidBluetoothAdminPermission() && HasAndroidFineLocationPermission();
            }
            
            return HasAndroidBluetoothPermission() && HasAndroidBluetoothAdminPermission() && HasAndroidFineLocationPermission() ;
        }

        private string[] GetMissingPermissions()
        {
            var permissionsList = new List<string>();
            if (!HasAndroidBluetoothPermission())
            {
                permissionsList.Add(Manifest.Permission.Bluetooth);
            }
            if (!HasAndroidBluetoothAdminPermission())
            {
                permissionsList.Add(Manifest.Permission.BluetoothAdmin);
            }

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q && !HasAndroidBackgroundLocationPermission())
            {
                permissionsList.Add(Manifest.Permission.AccessBackgroundLocation);
            }
            if (!HasAndroidFineLocationPermission())
            {
                permissionsList.Add(Manifest.Permission.AccessFineLocation);
            }

            if (!HasAndroidCoarseLocationPermission())
            {
                permissionsList.Add(Manifest.Permission.AccessCoarseLocation);
            }
            return permissionsList.ToArray();
        }

        private bool HasAndroidBluetoothPermission()
        {
            return ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                       Manifest.Permission.Bluetooth) == Permission.Granted;
        }

        private bool HasAndroidBluetoothAdminPermission()
        {
            return ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                       Manifest.Permission.BluetoothAdmin) == Permission.Granted;
        }

        private bool HasAndroidBackgroundLocationPermission()
        {
            var permission = ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                    Manifest.Permission.AccessBackgroundLocation);
            if (permission == Android.Content.PM.Permission.Granted)
            {
                return true;
            }
            return false;
        }

        private bool HasAndroidFineLocationPermission()
        {
            var permission = ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                Manifest.Permission.AccessFineLocation);
            if (permission == Android.Content.PM.Permission.Granted)
            {
                return true;
            }
            return false;
        }

        private bool HasAndroidCoarseLocationPermission()
        {
            var permission = ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                Manifest.Permission.AccessCoarseLocation);
            if (permission == Android.Content.PM.Permission.Granted)
            {
                return true;
            }
            return false;
        }
        
        private void ShouldShowRequestPermissionRationale()
        {
            //ActivityCompat.ShouldShowRequestPermissionRationale();
            //TODO: Implement later if needed.
        }
    }
}