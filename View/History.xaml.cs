using Banking.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Banking.Model.Classes;

namespace Banking.View
{ 
    public sealed partial class History : Page
    {
        WriteAndRead ListHistory = new WriteAndRead();
        public History()
        {
            InitializeComponent();
        }

        private async void ListViewHistory_Loading(FrameworkElement sender, object args)
        {
           
            ListViewHistory.ItemsSource = await ListHistory.GetListHistory();
        }

        private void BtnToMain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
