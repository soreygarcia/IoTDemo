using IoTDemo.Infrastructure;
using IoTDemo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IoTDemo.ViewModels
{
    public class EventViewModel : BindableBase
    {
        private string description;

        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { Set(ref date, value); }
        }
    }
}
