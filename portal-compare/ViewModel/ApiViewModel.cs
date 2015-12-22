﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using portal_compare.Helpers;
using portal_compare.Model.Apis;

namespace portal_compare.ViewModel
{
    public class ApiViewModel : ViewModelBase
    {
        private ObservableCollection<string> _source;
        private string _sourceDifferences;
        private ObservableCollection<string> _target;
        private string _targetDifferences;

        public ApiViewModel()
        {
            CompareCommand = new RelayCommand(Compare);
        }

        public RelayCommand CompareCommand { get; private set; }

        public ObservableCollection<string> Source
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }

        public string SourceDifferences
        {
            get { return _sourceDifferences; }
            set
            {
                _sourceDifferences = value;
                OnPropertyChanged(nameof(SourceDifferences));
            }
        }

        public ObservableCollection<string> Target
        {
            get { return _target; }
            set
            {
                _target = value;
                OnPropertyChanged(nameof(Target));
            }
        }

        public string TargetDifferences
        {
            get { return _targetDifferences; }
            set
            {
                _targetDifferences = value;
                OnPropertyChanged(nameof(TargetDifferences));
            }
        }

        private void Compare(object notUsed)
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
                HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);

                ApiWrapper sourceApi = sourceClient.Get<ApiWrapper>("/apis");
                ApiWrapper targetApi = targetClient.Get<ApiWrapper>("/apis");

                if (sourceApi?.value != null && targetApi?.value != null)
                {
                    Source = new ObservableCollection<string>(); Target = new ObservableCollection<string>();
                    Target = new ObservableCollection<string>();

                    SourceDifferences = $"There are {sourceApi.value.Count()} Api in the source system.";
                    TargetDifferences = $"There are {targetApi.value.Count()} Api in the target system.";
                    int sourceDifferences = 0;
                    int targetDifferences = 0;

                    foreach (Api sourceGroup in sourceApi.value)
                    {
                        Api target = targetApi.value.FirstOrDefault(t => t.Equals(sourceGroup));
                        if (target == null)
                        {
                            sourceDifferences += 1;
                            Source.Add(sourceGroup.ToString());
                        }
                    }

                    foreach (Api targetGroup in targetApi.value)
                    {
                        Api source = sourceApi.value.FirstOrDefault(t => t.Equals(targetGroup));
                        if (source == null)
                        {
                            targetDifferences += 1;
                            Target.Add(targetGroup.ToString());
                        }
                    }

                    SourceDifferences += Environment.NewLine + $"There are {sourceDifferences} Api that exist in the source system, but not in the target.";
                    TargetDifferences += Environment.NewLine + $"There are {targetDifferences} Api that exist in the target system, but not in the source.";
                }
            }
        }
    }
}