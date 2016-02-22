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
            LoadApis();
        }

        private void LoadApis()
        {
            if (App.Credentials == null)
            {
                if (App.TabControl != null)
                {
                    object tab = App.TabControl.Items[0];
                    App.TabControl.SelectedItem = tab;
                }
                MessageBox.Show("Please enter the API Management Credentials", "Error", MessageBoxButton.OK);
            }
            else
            {
                try
                {
                    HttpHelper sourceClient = new HttpHelper(App.Credentials.SourceServiceName, App.Credentials.SourceId, App.Credentials.SourceKey);
                    ApiWrapper sourceApi = sourceClient.Get<ApiWrapper>("/apis");
                    if (sourceApi != null)
                        ApiList = new ObservableCollection<Api>(sourceApi.value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
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

                try
                {
                    HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);
                    ApiWrapper targetApi = targetClient.Get<ApiWrapper>("/apis");
                    if (targetApi?.value != null && targetApi.value.Any())
                    {
                        App.TargetApi = targetApi.value.FirstOrDefault(t => t.name.Equals(_currentApi.name, StringComparison.OrdinalIgnoreCase));
                        if (App.TargetApi == null)
                        {
                            MessageBox.Show($"The API {_currentApi.name} doesn't exist in the target system");
                        }
                        else
                        {
                            if (App.LoadApiOperations != null)
                            {
                                App.LoadApiOperations.Execute();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
    }
}
