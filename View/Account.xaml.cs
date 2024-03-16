using Banking.View;
using Banking.ViewModel;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Banking.Model.Classes;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Banking
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<APIValutes> listAPI = new List<APIValutes>();
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new API();
        }

        private async void ComboBoxValute_Loading(FrameworkElement sender, object args)
        {
            API API = new API();
            listAPI = await API.GetValutes();
            ComboBoxValute.ItemsSource = listAPI;
            ComboBoxValute.DisplayMemberPath = "Valute";
            ComboBoxValute.SelectedValuePath = "Value";

            ComboBoxValute.SelectedIndex = 13;
        }

        private void BtnToOperation_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Operations));
        }

        private void BtnToHistory_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(History));
        }
    }
}
