using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using portal_compare.Helpers;
using portal_compare.Model;
using portal_compare.Model.Apis;

namespace portal_compare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Credentials Credentials { get; set; }
        public static Api SourceApi { get; set; }
        public static TabControl TabControl { get; set; }
        public static Api TargetApi { get; set; }

        public static RelayCommand LoadApiOperations;
        public static RelayCommand CompareApiCommand;
        public static RelayCommand CompareGroupCommand;
        public static RelayCommand CompareProductCommand;
        public static int CurrentTab = 0;
    }
}
