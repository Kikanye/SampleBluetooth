using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SampleBluetooth
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// call when page is appearing
        /// </summary>
        public virtual void OnAppearing()
        {
            // no default implementation
        }

        /// <summary>
        /// called when viewmodel page is disappearing
        /// </summary>
        public virtual void OnDisappearing()
        {
            // no default implementation
        }

    }
}
