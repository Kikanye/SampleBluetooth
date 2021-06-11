using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleBluetooth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothDevicesPage : ContentPage
    {
        public BluetoothDevicesPage()
        {
            InitializeComponent();
            BindingContext = new BluetoothDevicesPageViewModel(new PageNavService(this));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as BluetoothDevicesPageViewModel;
            if (vm != null)
            {
               
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var vm = BindingContext as BluetoothDevicesPageViewModel;
            if (vm != null)
            {
                
            }
        }
    }
}