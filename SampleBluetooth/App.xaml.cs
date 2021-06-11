using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleBluetooth
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new BluetoothDevicesPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
