using Banking.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Banking.Model.Classes;

namespace Banking.View
{
    public sealed partial class Operations : Page
    {
        readonly API API = new API();
        public Operations()
        {
            InitializeComponent();
            DataContext = new API();
        }

        private async void ComboBoxValute_Loading(FrameworkElement sender, object args)
        {
            ComboBoxValute.ItemsSource = await API.GetValutes();
            ComboBoxValute.DisplayMemberPath = "Valute";
            ComboBoxValute.SelectedValuePath = "Value";
            ComboBoxValute.SelectedIndex = 13;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedType = ComBoxTypeOperation.SelectedItem as ComboBoxItem;
            string selectedTypeKey = selectedType.Content.ToString();
            APIValutes selectedValute = ComboBoxValute.SelectedItem as APIValutes;
            string selectedValuteKey = selectedValute.Valute;
            WriteAndRead WriteInDB = new WriteAndRead();
            await WriteInDB.Write(TBoxSum.Text, selectedTypeKey, selectedValuteKey);
        }

    }

}
