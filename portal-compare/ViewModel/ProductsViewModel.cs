using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using portal_compare.Helpers;
using portal_compare.Model.Groups;
using portal_compare.Model.Products;

namespace portal_compare.ViewModel
{
    public class ProductsViewModel : ViewModelBase
    {
        private string _sourceDifferences;
        private ObservableCollection<string> _source;
        private string _targetDifferences;
        private ObservableCollection<string> _target;

        public ProductsViewModel()
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

        public ObservableCollection<string> Source
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
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

        public ObservableCollection<string> Target
        {
            get { return _target; }
            set
            {
                _target = value;
                OnPropertyChanged(nameof(Target));
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

                ProductWrapper sourceProduct = sourceClient.Get<ProductWrapper>("/products");
                ProductWrapper targetProduct = targetClient.Get<ProductWrapper>("/products");

                if (sourceProduct?.value != null && targetProduct?.value != null)
                {
                    Source = new ObservableCollection<string>();
                    Target = new ObservableCollection<string>();

                    SourceDifferences = $"There are {sourceProduct.value.Count()} product in the source system.";
                    TargetDifferences = $"There are {targetProduct.value.Count()} product in the target system.";
                    int sourceDifferences = 0;
                    int targetDifferences = 0;

                    foreach (Product sourceGroup in sourceProduct.value)
                    {
                        Product target = targetProduct.value.FirstOrDefault(t => t.Equals(sourceGroup));
                        if (target == null)
                        {
                            sourceDifferences += 1;
                            Source.Add(sourceGroup.ToString());
                        }
                    }

                    foreach (Product targetGroup in targetProduct.value)
                    {
                        Product source = sourceProduct.value.FirstOrDefault(t => t.Equals(targetGroup));
                        if (source == null)
                        {
                            targetDifferences += 1;
                            Target.Add(targetGroup.ToString());
                        }
                    }

                    SourceDifferences += Environment.NewLine + $"There are {sourceDifferences} product that exist in the source system, but not in the target.";
                    TargetDifferences += Environment.NewLine + $"There are {targetDifferences} product that exist in the target system, but not in the source.";
                }
            }
        }
    }
}
