using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using portal_compare.Helpers;
using portal_compare.Model.Apis;
using portal_compare.Model.Products;

namespace portal_compare.ViewModel
{
    public class ApiViewModel : ViewModelBase
    {
        private ObservableCollection<string> _source;
        private string _sourceDifferences;
        private ObservableCollection<string> _target;
        private string _targetDifferences;
        private List<Api> _sourceList;
        private List<Api> _targetList;

        public ApiViewModel()
        {
            Compare();
            AddToTargetCommand = new RelayCommand(AddToTarget);
        }


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
                    ApiWrapper sourceApi = sourceClient.Get<ApiWrapper>("/apis");
                    ApiWrapper targetApi = targetClient.Get<ApiWrapper>("/apis");

                    if (sourceApi?.value != null && targetApi?.value != null)
                    {
                        Source = new ObservableCollection<string>(); Target = new ObservableCollection<string>();
                        Target = new ObservableCollection<string>();
                        _sourceList = sourceApi.value.ToList();
                        _targetList = targetApi.value.ToList();

                        SourceDifferences = $"{sourceApi.value.Count()} API(s) in the source system.";
                        TargetDifferences = $"{targetApi.value.Count()} API(s) in the target system.";
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

                        SourceDifferences += Environment.NewLine + $"{sourceDifferences} API(s) that is different in target.";
                        TargetDifferences += Environment.NewLine + $"{targetDifferences} API(s) that is different in source.";
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
                try
                {
                    Api original = _sourceList.FirstOrDefault(t => t.ToString().Equals(name.ToString(), StringComparison.OrdinalIgnoreCase));
                    if (original != null)
                    {
                        Api target = _targetList.FirstOrDefault(t => t.name.Equals(original.name.ToString(), StringComparison.OrdinalIgnoreCase));
                        HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);
                        if (target == null)
                        {
                            //Adding new
                            MessageBoxResult result = MessageBox.Show($"Are you sure you want to add '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                //https://msdn.microsoft.com/en-us/library/azure/dn781423.aspx#CreateAPI
                                targetClient.Put($"{original.id}", original);
                                Compare();
                                MessageBox.Show("API has been added.");
                            }
                        }
                        else
                        {
                            //Update existing
                            MessageBoxResult result = MessageBox.Show($"Are you sure you want to update '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                //https://msdn.microsoft.com/en-us/library/azure/dn781423.aspx#UpdateAPI
                                target.name = original.name;
                                target.description = original.description;
                                target.path = original.path;
                                target.protocols = original.protocols;
                                target.subscriptionKeyParameterNames = original.subscriptionKeyParameterNames;
                                targetClient.Patch($"{target.id}", target);
                                Compare();
                                MessageBox.Show("API has been updated.");
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