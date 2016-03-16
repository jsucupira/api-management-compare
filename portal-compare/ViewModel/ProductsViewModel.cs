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
        private List<Product> _sourceList;
        private List<Product> _targetList;

        public ProductsViewModel()
        {
            AddToTargetCommand = new RelayCommand(AddToTarget);
            App.CompareProductCommand = new RelayCommand(Compare);
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

        private void Compare(object notUsed = null)
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
                    ProductWrapper sourceProduct = sourceClient.Get<ProductWrapper>("/products");
                    ProductWrapper targetProduct = targetClient.Get<ProductWrapper>("/products");

                    if (sourceProduct?.value != null && targetProduct?.value != null)
                    {
                        Source = new ObservableCollection<string>();
                        Target = new ObservableCollection<string>();
                        _sourceList = sourceProduct.value.ToList();
                        _targetList = targetProduct.value.ToList();

                        SourceDifferences = $"{sourceProduct.value.Count()} product(s) in the source system.";
                        TargetDifferences = $"{targetProduct.value.Count()} product(s) in the target system.";
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

                        SourceDifferences += Environment.NewLine + $"{sourceDifferences} product(s) that is different in target.";
                        TargetDifferences += Environment.NewLine + $"{targetDifferences} product(s) that is different in source.";
                    }
                }
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "Error");
                //}
                finally {
                    
                }
            }
        }

        public RelayCommand AddToTargetCommand { get; set; }

        private void AddToTarget(object name)
        {
            if (!string.IsNullOrEmpty(name as string))
            {
                Product original = _sourceList.FirstOrDefault(t => t.ToString().Equals(name.ToString(), StringComparison.OrdinalIgnoreCase));
                if (original != null)
                {
                    Product target = _targetList.FirstOrDefault(t => t.name.Equals(original.name.ToString(), StringComparison.OrdinalIgnoreCase));
                    HttpHelper targetClient = new HttpHelper(App.Credentials.TargetServiceName, App.Credentials.TargetId, App.Credentials.TargetKey);
                    if (target == null)
                    {
                        //Adding new
                        MessageBoxResult result = MessageBox.Show($"Are you sure you want to add '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                //https://msdn.microsoft.com/en-us/library/azure/dn776336.aspx?f=255&MSPPError=-2147217396#CreateProduct
                                targetClient.Put($"{original.id}", original);
                                Compare();
                                MessageBox.Show("Product has been added.");
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
                        MessageBoxResult result = MessageBox.Show($"Are you sure you want to update '{original.name}' to the target environment?", "Warning", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                //https://msdn.microsoft.com/en-us/library/azure/dn776336.aspx?f=255&MSPPError=-2147217396#UpdateProduct
                                target.name = original.name;
                                target.description = original.description;
                                target.terms = original.terms;
                                target.subscriptionRequired = original.subscriptionRequired;
                                target.approvalRequired = original.approvalRequired;
                                target.subscriptionsLimit = original.subscriptionsLimit;
                                target.state = original.state;
                                targetClient.Patch($"{target.id}", target);
                                Compare();
                                MessageBox.Show("Product has been updated.");
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
