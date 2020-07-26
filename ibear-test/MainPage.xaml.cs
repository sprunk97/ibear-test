using ibear_test.Database;
using ibear_test.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ibear_test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ContractorsContext())
            {
                lvContractors.ItemsSource = db.Contractors.ToList().OrderBy(x => x.Name);
            }
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lvContractors.SelectedIndex != -1)
            {
                var popup = new ContentDialog
                {
                    Content = "Удалить контрагента?",
                    PrimaryButtonText = "Удалить",
                    CloseButtonText = "Отмена"
                };
                var result = await popup.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    var todelete = lvContractors.SelectedItem as Contractor;
                    using (var db = new ContractorsContext())
                    {
                        db.Contractors.Remove(todelete);
                        await db.SaveChangesAsync();
                        lvContractors.ItemsSource = db.Contractors.ToList().OrderBy(x => x.Name);
                    }
                }
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void lvContractors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvContractors.SelectedIndex != -1)
            {
                var selected = lvContractors.SelectedItem as Contractor;
                if (selected.Photo == null) photo.Source = new BitmapImage(new Uri("ms-appx:///Assets/avatar-placeholder.png"));
                else photo.Source = await Conversion.ByteArrayToBitmapAsync(selected.Photo);
                name.Text = selected.Name;
                phone.Text = selected.Phone.ToString();
                if (selected.Email != null) email.Text = selected.Email;
                else email.Text = "";
            }
        }

        private void name_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(name.Text);
            Clipboard.SetContent(dataPackage);
        }

        private void phone_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(phone.Text);
            Clipboard.SetContent(dataPackage);
        }

        private void email_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (email.Text != "")
            {
                var dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                dataPackage.SetText(email.Text);
                Clipboard.SetContent(dataPackage);
            }
        }
    }
}
