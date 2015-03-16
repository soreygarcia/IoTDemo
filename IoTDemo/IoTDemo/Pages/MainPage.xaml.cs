using IoTDemo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IoTDemo.Pages
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
            InitializeComponent();
            BindingContext = ViewModelLocator.Main;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await ViewModelLocator.Main.LoadAsync();
        }
	}
}
