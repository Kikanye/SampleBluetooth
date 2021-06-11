using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleBluetooth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBluetoothDevicePage
    {
        public AddBluetoothDevicePage()
        {
            InitializeComponent();
            BindingContext = new AddBluetoothDevicePageViewModel(new PageNavService(this));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as AddBluetoothDevicePageViewModel;
            if (vm != null)
            {
                vm.ScanForDevicesCommand.Execute(null);
            }
        }
        
    }
}