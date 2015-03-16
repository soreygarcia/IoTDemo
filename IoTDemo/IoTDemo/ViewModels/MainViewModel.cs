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
    public class MainViewModel : BindableBase
    {
        ApiService apiService;
        DialogService dialogService;

        public MainViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();

            Events = new ObservableCollection<EventViewModel>();
        }

        public ObservableCollection<EventViewModel> Events { get; set; }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { Set(ref isBusy, value); }
        }
        
        public ICommand RetryCommand
        {
            get
            {
                return new RelayCommand(Retry);
            }
        }

        private async void Retry()
        {
            await LoadAsync();
        }

        internal async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                var response = await apiService.GetEvents();
                this.Events.Clear();

                foreach (var item in response)
                {
                    this.Events.Add(new EventViewModel() 
                    { 
                        Description = item.Description,
                        Date = item.CreatedAt
                    });
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowException(ex, "IoTDemo");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
