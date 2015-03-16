using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IoTDemo.Infrastructure
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Set<T>(ref T storage, T value = default(T), [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}