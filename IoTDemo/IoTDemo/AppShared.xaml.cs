using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IoTDemo
{
	public partial class AppShared : Application
	{
		public AppShared ()
		{
			InitializeComponent ();
            NavigationContainer = new NavigationPage(new IoTDemo.Pages.MainPage());
            MainPage = NavigationContainer;
		}


        public static NavigationPage NavigationContainer { get; private set; }
	}
}
