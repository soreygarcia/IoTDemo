using System;
using System.Collections.Generic;
using System.Text;
using IoTDemo.ViewModels;

namespace IoTDemo.Infrastructure
{
    public class ViewModelLocator
    {
        private static MainViewModel main;
        public static MainViewModel Main
        {
            get
            {
                if (main == null)
                    main = new MainViewModel();
                return main;
            }
        }
    }
}
