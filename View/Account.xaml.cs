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
        List<APIValutes> api = new List<APIValutes>();
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
            DBRequests db = new DBRequests();
            Dictionary<string, double> groups = new Dictionary<string, double>();
            List<Valute> list = await db.BDSelect();
            APIValutes selected = ComboBoxValute.SelectedItem as APIValutes;
            double selectedValue = selected.Value;
            double AllSumValue = 0;
            foreach (var item in list)
            {
                foreach (var itemAPI in api)
                {
                    if (item.Name.Contains(itemAPI.Valute))
                    {
                        if (item.Type == "Зачисление")
                        {
                            AllSumValue += item.Value * itemAPI.Value;
                            break;
                        }
                        else if (item.Type == "Снятие")
                        {
                            AllSumValue -= item.Value * itemAPI.Value;
                            break;
                        }
                    }
                }

            }

            Block.Text = Math.Round(AllSumValue / selectedValue, 2).ToString();

        }

        private async void ComboBoxValute_Loaded(object sender, RoutedEventArgs e)
        {
            API API = new API();
            api = await API.GetValutes();
            ComboBoxValute.ItemsSource = api;
            ComboBoxValute.DisplayMemberPath = "Valute";
            ComboBoxValute.SelectedValuePath = "Value";
            ComboBoxValute.SelectedIndex = 13;
        }
    }

}
