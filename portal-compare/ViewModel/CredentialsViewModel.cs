using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using portal_compare.Helpers;
using portal_compare.Model;

namespace portal_compare.ViewModel
{
    public class CredentialsViewModel : ViewModelBase
    {
        private bool _save;
        private string _sourceId;
        private string _sourceKey;
        private string _sourceServiceName;
        private string _targetId;
        private string _targetKey;
        private string _targetServiceName;

        public CredentialsViewModel()
        {
            string pathDirectory = Directory.GetCurrentDirectory();
            string fileName = $@"{pathDirectory}\credentials";
            if (File.Exists(fileName))
            {
                try
                {
                    App.Credentials = JsonConvert.DeserializeObject<Credentials>(File.ReadAllText(fileName));
                }
                catch (Exception)
                {
                    File.Delete(fileName);
                }
            }
            if (App.Credentials != null)
            {
                SourceServiceName = App.Credentials.SourceServiceName;
                SourceId = App.Credentials.SourceId;
                SourceKey = App.Credentials.SourceKey;
                TargetServiceName = App.Credentials.TargetServiceName;
                TargetId = App.Credentials.TargetId;
                TargetKey = App.Credentials.TargetKey;
            }

            SaveCommand = new RelayCommand(SetCredentials);
        }

        public bool Save
        {
            get { return _save; }
            set
            {
                _save = value;
                OnPropertyChanged(nameof(Save));
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        public string SourceId
        {
            get { return _sourceId; }
            set
            {
                _sourceId = value;
                OnPropertyChanged(nameof(SourceId));
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

        public string SourceServiceName
        {
            get { return _sourceServiceName; }
            set
            {
                _sourceServiceName = value;
                OnPropertyChanged(nameof(SourceServiceName));
            }
        }

        public string TargetId
        {
            get { return _targetId; }
            set
            {
                _targetId = value;
                OnPropertyChanged(nameof(TargetId));
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

        public string TargetServiceName
        {
            get { return _targetServiceName; }
            set
            {
                _targetServiceName = value;
                OnPropertyChanged(nameof(TargetServiceName));
            }
        }

        private void SetCredentials(object obj)
        {
            App.Credentials = new Credentials
            {
                SourceId = SourceId,
                SourceKey = SourceKey,
                SourceServiceName = SourceServiceName,
                TargetId = TargetId,
                TargetKey = TargetKey,
                TargetServiceName = TargetServiceName
            };
            string pathDirectory = Directory.GetCurrentDirectory();
            string fileName = $@"{pathDirectory}\credentials";

            if (Save)
            {
                string json = JsonConvert.SerializeObject(App.Credentials);
                File.WriteAllText(fileName, json);
            }
            else
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }

            MessageBox.Show("Saved", "Success", MessageBoxButton.OK);
        }
    }
}