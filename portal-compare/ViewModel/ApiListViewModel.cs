using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using portal_compare.Helpers;
using portal_compare.Model.Apis;

namespace portal_compare.ViewModel
{
    public class ApiListViewModel : ViewModelBase
    {
        private ObservableCollection<Api> _apiList;
        private Api _currentApi;

        public ObservableCollection<Api> ApiList
        {
            get { return _apiList; }
            set
            {
                _apiList = value;
                OnPropertyChanged(nameof(ApiList));
            }
        }

        public ApiListViewModel()
        {
            LoadApiCommand = new RelayCommand(LoadApis);
        }

        public RelayCommand LoadApiCommand { get; set; }

        private void LoadApis(object notUsed)
        {
            if (App.Credentials == null)
            {
                object tab = App.TabControl.Items[0];
                App.TabControl.SelectedItem = tab;
                MessageBox.Show("Please enter the API Management Credentials", "Error", MessageBoxButton.OK);
            }
            else
            {
                HttpHelper sourceClient = new HttpHelper(App.Credentials.SourceServiceName, App.Credentials.SourceId, App.Credentials.SourceKey);
                ApiWrapper sourceApi = sourceClient.Get<ApiWrapper>("/apis");
                if (sourceApi != null)
                    ApiList = new ObservableCollection<Api>(sourceApi.value);
            }
        }

        public Api CurrentApi
        {
            get { return _currentApi; }
            set
            {
                _currentApi = value;
                App.SourceApi = _currentApi;
                OnPropertyChanged(nameof(CurrentApi));

                HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);
                ApiWrapper targetApi = targetClient.Get<ApiWrapper>("/apis");
                if (targetApi?.value != null && targetApi.value.Any())
                {
                    App.TargetApi = targetApi.value.FirstOrDefault(t => t.Equals(_currentApi));
                    if (App.TargetApi == null)
                    {
                        MessageBox.Show($"The API {_currentApi.name} doesn't exist in the target system");
                    }
                }
            }
        }
    }
}
