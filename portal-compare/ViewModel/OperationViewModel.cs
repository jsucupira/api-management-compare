using System;
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
        private ObservableCollection<string> _target;
        private string _targetDifferences;

        public OperationViewModel()
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

                OperationWrapper sourceOperationWrapper = sourceClient.Get<OperationWrapper>($"{App.SourceApi.id}?export=true");
                
                OperationWrapper targetOperationWrapper = targetClient.Get<OperationWrapper>($"{App.TargetApi.id}?export=true");

                if (sourceOperationWrapper?.operations != null && targetOperationWrapper?.operations!= null)
                {
                    var sourceOperation = sourceOperationWrapper.operations;
                    var targetOperation = targetOperationWrapper.operations;

                    if (sourceOperation?.value != null && targetOperation?.value != null)
                    {
                        Source = new ObservableCollection<string>();
                        Target = new ObservableCollection<string>();

                        SourceDifferences = $"There are {sourceOperation.value.Count()} Operation in the source system.";
                        TargetDifferences = $"There are {targetOperation.value.Count()} Operation in the target system.";
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

                        SourceDifferences += Environment.NewLine + $"There are {sourceDifferences} Operation that exist in the source system, but not in the target.";
                        TargetDifferences += Environment.NewLine + $"There are {targetDifferences} Operation that exist in the target system, but not in the source.";
                    }
                }
            }
        }
    }
}