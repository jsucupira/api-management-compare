using System;
using System.Collections.Generic;
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
        private List<Group> _sourceList;
        private List<Group> _targetList;

        public GroupsViewModel()
        {
            Compare();
            AddToTargetCommand = new RelayCommand(AddToTarget);
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

        private void Compare()
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

                try
                {
                    GroupWrapper sourceGroups = sourceClient.Get<GroupWrapper>("/groups");
                    GroupWrapper targetGroups = targetClient.Get<GroupWrapper>("/groups");

                    if (sourceGroups?.value != null && targetGroups?.value != null)
                    {
                        TargetGroups = new ObservableCollection<string>();
                        SourceGroups = new ObservableCollection<string>();
                        _sourceList = sourceGroups.value.ToList();
                        _targetList = targetGroups.value.ToList();

                        SourceDifferences = $"{sourceGroups.value.Count()} group(s) in the source system.";
                        TargetDifferences = $"{targetGroups.value.Count()} group(s) in the target system.";
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

                        SourceDifferences += Environment.NewLine + $"{sourceDifferences} group(s) that is different in the target.";
                        TargetDifferences += Environment.NewLine + $"{targetDifferences} group(s) that is different in the source.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        public RelayCommand AddToTargetCommand { get; set; }

        private void AddToTarget(object name)
        {
            if (!string.IsNullOrEmpty(name as string))
            {
                Group original = _sourceList.FirstOrDefault(t => t.ToString().Equals(name.ToString(), StringComparison.OrdinalIgnoreCase));
                if (original != null)
                {
                    Group target = _targetList.FirstOrDefault(t => t.name.Equals(original.name.ToString(), StringComparison.OrdinalIgnoreCase));
                    HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);
                    if (target == null)
                    {
                        //Adding new
                        MessageBoxResult result = MessageBox.Show($"Are you sure you want to add the group: '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            //https://msdn.microsoft.com/en-us/library/azure/dn776329.aspx#CreateGroup
                            try
                            {
                                targetClient.Put($"{original.id}", original.Map());
                                Compare();
                                MessageBox.Show("Group has been added.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                        }
                    }
                    else
                    {
                        //Update existing
                        MessageBoxResult result = MessageBox.Show($"Are you sure you want to update the group: '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            //https://msdn.microsoft.com/en-us/library/azure/dn776329.aspx#UpdateGroup
                            target.name = original.name;
                            target.description = original.description;
                            try
                            {
                                targetClient.Patch($"{target.id}", target.Map());
                                Compare();
                                MessageBox.Show("Group has been updated.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                        }
                    }
                }
            }
        }


    }
}