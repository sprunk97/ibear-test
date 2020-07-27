using ibear_test.Database;
using ibear_test.Tools;
using Microsoft.EntityFrameworkCore;
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != "")
            {
                var pars = (Contractor)e.Parameter;
                using (var db = new ContractorsContext())
                {
                    db.Entry(pars).State = pars.ID == Guid.Empty ? EntityState.Added : EntityState.Modified;
                    await db.SaveChangesAsync();
                    lvContractors.ItemsSource = db.Contractors.ToList().OrderBy(x => x.Name);
                }
            }
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditPage));
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
            if (lvContractors.SelectedIndex != -1)
            {
                var selected = lvContractors.SelectedItem as Contractor;
                var pars = new Contractor
                {
                    Photo = selected.Photo,
                    Name = selected.Name,
                    Email = selected.Email,
                    Phone = selected.Phone
                };
                Frame.Navigate(typeof(EditPage), pars);
            }
        }

        private async void lvContractors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvContractors.SelectedIndex != -1)
            {
                var selected = lvContractors.SelectedItem as Contractor;
                if (selected.Photo == null)
                {
                    var bi = new BitmapImage(new Uri("ms-appx:///Assets/avatar-placeholder.png"));
                    photo.Source = bi;
                    photo.Source = await (photo.Source as BitmapImage).AsWriteableBitmapAsync();
                }
                else photo.Source = await selected.Photo.AsWriteableBitmapAsync();

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
