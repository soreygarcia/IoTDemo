using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoTDemo.Services
{
    public class DialogService 
    {
        public async Task ShowMessage(string message, string title)
        {
            await AppShared.NavigationContainer.DisplayAlert(title, message, "Aceptar");
        }

        public async Task ShowException(Exception error, string title)
        {
            await AppShared.NavigationContainer.DisplayAlert(title, error.Message, "Aceptar");
        }

        public async Task<bool> ShowConfirm(string message, string title)
        {
            return await AppShared.NavigationContainer.DisplayAlert(title, message, "Si", "No");
        }

        public async Task<string> ShowList(string title, List<string> names)
        {
            string[] items = new string[names.Count];
            int i = 0;
            foreach (var item in names)
            {
                items[i] = item;
                i++;
            }

            return await AppShared.NavigationContainer.DisplayActionSheet(title, "Cancelar", null, items);
        }
    }
}
