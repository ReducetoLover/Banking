using Banking.DB;
using Banking.View;
using Banking.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Banking.Model.Classes;

namespace Banking
{
    public sealed partial class MainPage : Page
    {
        DBRequests db = new DBRequests();
        List<APIValutes> ListApi = new List<APIValutes>();
        WriteAndRead ListSum = new WriteAndRead();
        readonly API API = new API();
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new API();
        }

        private void BtnToOperation_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Operations));
        }

        private void BtnToHistory_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(History));
        }

        private async void ComboBoxValute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Block.Text = "0";
            APIValutes selected = ComboBoxValute.SelectedItem as APIValutes;
            double selectedValue = selected.Value;
            Block.Text = await ListSum.GetSumInValute(selectedValue, ListApi);
        }

        private async void ComboBoxValute_Loaded(object sender, RoutedEventArgs e)
        {
            ListApi = await API.GetValutes();
            ComboBoxValute.ItemsSource = ListApi;
            ComboBoxValute.DisplayMemberPath = "Valute";
            ComboBoxValute.SelectedValuePath = "Value";
            ComboBoxValute.SelectedIndex = 13;
        }

        private void Account_Loading(FrameworkElement sender, object args)
        {
            db.BDCreate();
        }
    }

}
