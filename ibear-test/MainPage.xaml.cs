using ibear_test.Database;
using ibear_test.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
            if (e.Parameter.GetType() != typeof(string))
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
                    ID = selected.ID,
                    Photo = selected.Photo,
                    Name = selected.Name,
                    Email = selected.Email,
                    Phone = selected.Phone,
                    Width = selected.Width,
                    Height = selected.Height
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
                }
                else
                    photo.Source = await selected.Photo.AsWBAsync(selected.Width, selected.Height);

                name.Text = selected.Name;
                phone.Text = selected.Phone.ToString();
                if (selected.Email != null)
                    email.Text = selected.Email;
                else
                    email.Text = "";
            }
        }

        private void CopyToClipboard(object sender, TappedRoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText((sender as TextBlock).Text);
            Clipboard.SetContent(dataPackage);
        }
    }
}
