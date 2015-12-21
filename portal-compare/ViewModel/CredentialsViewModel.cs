using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using portal_compare.Helpers;
using portal_compare.Model;

namespace portal_compare.ViewModel
{
    public class CredentialsViewModel : ViewModelBase
    {
        private string _sourceServiceName;
        private string _sourceApi;
        private string _sourceKey;
        private string _targetServiceName;
        private string _targetApi;
        private string _targetKey;

        public CredentialsViewModel()
        {
            if (App.Credentials != null)
            {
                SourceServiceName = App.Credentials.SourceServiceName;
                SourceApi = App.Credentials.SourceId;
                SourceKey = App.Credentials.SourceKey;
                TargetServiceName = App.Credentials.TargetServiceName;
                TargetApi = App.Credentials.TargetApi;
                TargetKey = App.Credentials.TargetKey;
            }

            SaveCommand = new RelayCommand(SetCredentials);
        }

        private void SetCredentials(object obj)
        {
            App.Credentials = new Credentials
            {
                SourceId = SourceApi,
                SourceKey = SourceKey,
                SourceServiceName = SourceServiceName,
                TargetApi = TargetApi,
                TargetKey = TargetKey,
                TargetServiceName = TargetServiceName
            };
        }

        public RelayCommand SaveCommand { get; private set; }

        public string SourceServiceName
        {
            get { return _sourceServiceName; }
            set
            {
                _sourceServiceName = value;
                OnPropertyChanged(nameof(SourceServiceName));
            }
        }

        public string SourceApi
        {
            get { return _sourceApi; }
            set
            {
                _sourceApi = value;
                OnPropertyChanged(nameof(SourceApi));
            }
        }

        public string SourceKey
        {
            get { return _sourceKey; }
            set
            {
                _sourceKey = value;
                OnPropertyChanged(nameof(SourceKey));
            }
        }

        public string TargetServiceName
        {
            get { return _targetServiceName; }
            set
            {
                _targetServiceName = value;
                OnPropertyChanged(nameof(TargetServiceName));
            }
        }

        public string TargetApi
        {
            get { return _targetApi; }
            set
            {
                _targetApi = value;
                OnPropertyChanged(nameof(TargetApi));
            }
        }

        public string TargetKey
        {
            get { return _targetKey; }
            set
            {
                _targetKey = value;
                OnPropertyChanged(nameof(TargetKey));
            }
        }
    }
}
