﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using portal_compare.Helpers;
using portal_compare.Model.Groups;

namespace portal_compare.ViewModel
{
    public class GroupsViewModel : ViewModelBase
    {
        private string _sourceDifferences;
        private ObservableCollection<string> _sourceGroups;
        private string _targetDifferences;
        private ObservableCollection<string> _targetGroups;

        public GroupsViewModel()
        {
            CompareCommand = new RelayCommand(Compare);
        }

        public RelayCommand CompareCommand { get; private set; }

        public string SourceDifferences
        {
            get { return _sourceDifferences; }
            set
            {
                _sourceDifferences = value;
                OnPropertyChanged(nameof(SourceDifferences));
            }
        }

        public ObservableCollection<string> SourceGroups
        {
            get { return _sourceGroups; }
            set
            {
                _sourceGroups = value;
                OnPropertyChanged(nameof(SourceGroups));
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

        public ObservableCollection<string> TargetGroups
        {
            get { return _targetGroups; }
            set
            {
                _targetGroups = value;
                OnPropertyChanged(nameof(TargetGroups));
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

                GroupWrapper sourceGroups = sourceClient.Get<GroupWrapper>("/groups");
                GroupWrapper targetGroups = targetClient.Get<GroupWrapper>("/groups");

                if (sourceGroups?.value != null && targetGroups?.value != null)
                {
                    TargetGroups = new ObservableCollection<string>();
                    SourceGroups = new ObservableCollection<string>();
                    SourceDifferences = $"There are {sourceGroups.value.Count()} groups in the source system.";
                    TargetDifferences = $"There are {targetGroups.value.Count()} groups in the target system.";
                    int sourceDifferences = 0;
                    int targetDifferences = 0;

                    foreach (Group sourceGroup in sourceGroups.value)
                    {
                        Group target = targetGroups.value.FirstOrDefault(t => t.Equals(sourceGroup));
                        if (target == null)
                        {
                            sourceDifferences += 1;
                            SourceGroups.Add(sourceGroup.ToString());
                        }
                    }

                    foreach (Group targetGroup in targetGroups.value)
                    {
                        Group source = sourceGroups.value.FirstOrDefault(t => t.Equals(targetGroup));
                        if (source == null)
                        {
                            targetDifferences += 1;
                            TargetGroups.Add(targetGroup.ToString());
                        }
                    }

                    SourceDifferences += Environment.NewLine + $"There are {sourceDifferences} groups that exist in the source system, but not in the target.";
                    TargetDifferences += Environment.NewLine + $"There are {targetDifferences} groups that exist in the target system, but not in the source.";
                }
            }
        }
    }
}