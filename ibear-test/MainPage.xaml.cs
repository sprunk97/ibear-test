using ibear_test.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

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

        private void lvContractors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
