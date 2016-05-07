using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using portal_compare.Helpers;
using portal_compare.Model.Operations;

namespace portal_compare.ViewModel
{
    public class OperationViewModel : ViewModelBase
    {
        private ObservableCollection<string> _source;
        private string _sourceDifferences;
        private List<Operation> _sourceList;
        private ObservableCollection<string> _target;
        private string _targetDifferences;
        private List<Operation> _targetList;

        public OperationViewModel()
        {
            AddToTargetCommand = new RelayCommand(AddToTarget);
            MergeAllCommand = new RelayCommand(MergeAll);
            App.LoadApiOperations = new RelayCommand(Compare);
        }

        public RelayCommand AddToTargetCommand { get; set; }

        public RelayCommand MergeAllCommand { get; set; }

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

        private void AddOperation(string name, bool askConfirmation)
        {
            Operation original = _sourceList.FirstOrDefault(t => t.ToString().Equals(name.ToString(), StringComparison.OrdinalIgnoreCase));
            HttpHelper sourceClient = new HttpHelper(App.Credentials.SourceServiceName, App.Credentials.SourceId, App.Credentials.SourceKey);

            if (original != null)
            {
                Operation target = _targetList.FirstOrDefault(t => t.name.Equals(original.name.ToString(), StringComparison.OrdinalIgnoreCase));
                HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);
                if (target == null)
                {
                    //Adding new
                    bool result = true;
                    if (askConfirmation)
                    {
                        result = MessageBox.Show($"Are you sure you want to add '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                    }

                    if (result)
                    {
                        string id = original.id.Replace(App.SourceApi.id, App.TargetApi.id);
                        target = new Operation(id);
                        target.name = original.name;
                        target.description = original.description;
                        target.method = original.method;
                        target.urlTemplate = original.urlTemplate;
                        target.request = original.request;
                        target.responses = original.responses;
                        try
                        {
                            //https://msdn.microsoft.com/en-us/library/azure/dn781423.aspx#CreateOperation
                            targetClient.Put($"{target.id}", original);

                            if (sourceClient.CheckForPolicy(original.id))
                            {
                                CheckPolicy(original.id, target.id);
                            }

                            if (askConfirmation)
                            {
                                MessageBox.Show("Operation has been added.");
                                if (App.LoadApiOperations != null)
                                    App.LoadApiOperations.Execute();
                            }
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
                    bool result = true;
                    if (askConfirmation)
                        result = MessageBox.Show($"Are you sure you want to update '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

                    if (result)
                    {
                        //https://msdn.microsoft.com/en-us/library/azure/dn781423.aspx#UpdateOperation
                        target.name = original.name;
                        target.description = original.description;
                        target.method = original.method;
                        target.urlTemplate = original.urlTemplate;
                        target.request = original.request;
                        target.responses = original.responses;
                        try
                        {
                            targetClient.Patch($"{target.id}", target);

                            if (askConfirmation)
                            {
                                MessageBox.Show("Operation has been updated.");
                                if (App.LoadApiOperations != null)
                                    App.LoadApiOperations.Execute();
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

        private static void CheckPolicy(string originalId, string targetId)
        {
            HttpHelper sourceClient = new HttpHelper(App.Credentials.SourceServiceName, App.Credentials.SourceId, App.Credentials.SourceKey);
            HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);
            string policy = sourceClient.GetPolicy(originalId);
            targetClient.SetPolicy(policy, targetId);
        }

        private void AddToTarget(object name)
        {
            if (!string.IsNullOrEmpty(name as string))
            {
                AddOperation(name as string, true);
            }
        }

        private void Compare(object notUsed)
        {
            if (App.Credentials == null)
            {
                MessageBox.Show("Please enter the API Management Credentials", "Error", MessageBoxButton.OK);
                object tab = App.TabControl.Items[0];
                App.TabControl.SelectedItem = tab;
            }
            else if (App.SourceApi == null || App.TargetApi == null)
            {
                MessageBox.Show("Please select the api.", "Error", MessageBoxButton.OK);
            }
            else
            {
                HttpHelper sourceClient = new HttpHelper(App.Credentials.SourceServiceName, App.Credentials.SourceId, App.Credentials.SourceKey);
                HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);

                try
                {
                    OperationWrapper sourceOperationWrapper = sourceClient.Get<OperationWrapper>($"{App.SourceApi.id}?export=true");
                    OperationWrapper targetOperationWrapper = targetClient.Get<OperationWrapper>($"{App.TargetApi.id}?export=true");

                    if (sourceOperationWrapper?.operations != null && targetOperationWrapper?.operations != null)
                    {
                        Operations sourceOperation = sourceOperationWrapper.operations;
                        Operations targetOperation = targetOperationWrapper.operations;

                        if (sourceOperation?.value != null && targetOperation?.value != null)
                        {
                            Source = new ObservableCollection<string>();
                            Target = new ObservableCollection<string>();
                            _sourceList = sourceOperation.value.ToList();
                            _targetList = targetOperation.value.ToList();

                            SourceDifferences = $"{sourceOperation.value.Count()} operation(s) in the source system.";
                            TargetDifferences = $"{targetOperation.value.Count()} operation(s) in the target system.";
                            int sourceDifferences = 0;
                            int targetDifferences = 0;

                            foreach (Operation sourceGroup in sourceOperation.value)
                            {
                                Operation target = targetOperation.value.FirstOrDefault(t => t.Equals(sourceGroup));
                                if (target == null)
                                {
                                    sourceDifferences += 1;
                                    Source.Add(sourceGroup.ToString());
                                }

                            }

                            foreach (Operation targetGroup in targetOperation.value)
                            {
                                Operation source = sourceOperation.value.FirstOrDefault(t => t.Equals(targetGroup));
                                if (source == null)
                                {
                                    targetDifferences += 1;
                                    Target.Add(targetGroup.ToString());
                                }
                            }

                            SourceDifferences += Environment.NewLine + $"{sourceDifferences} operation(s) that is different in target.";
                            TargetDifferences += Environment.NewLine + $"{targetDifferences} operation(s) that is different in source.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void MergeAll(object notUsed)
        {
            if (App.SourceApi == null || App.TargetApi == null)
            {
                MessageBox.Show("Please select the api.", "Error", MessageBoxButton.OK);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to merge all the operations?", "Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (Operation operation in _sourceList)
                    {
                        AddOperation(operation.ToString(), false);
                    }
                    if (App.LoadApiOperations != null)
                        App.LoadApiOperations.Execute();
                    MessageBox.Show("Merge Completed.");
                }
            }
        }
    }
}