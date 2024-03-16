using Banking.ViewModel;
using System;
using System.Collections.Generic;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Banking.View
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Operations : Page
    {
        public Operations()
        {
            this.InitializeComponent();
            DataContext = new API();
        }

        private async void ComboBoxValute_Loading(FrameworkElement sender, object args)
        {
            API API = new API();
            ComboBoxValute.ItemsSource = await API.GetValutes();
            ComboBoxValute.DisplayMemberPath = "Valute";
            ComboBoxValute.SelectedValuePath = "Value";
            ComboBoxValute.SelectedIndex = 13;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
