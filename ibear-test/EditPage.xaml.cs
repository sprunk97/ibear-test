using ibear_test.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ibear_test.Tools
{
    public sealed partial class EditPage : Page
    {
        private bool used_placeholder = true;
        private Guid guid = Guid.Empty;

        public EditPage()
        {
            this.InitializeComponent();
            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            BackButton.IsEnabled = Frame.CanGoBack;
            var selected = (Contractor)e.Parameter;

            var bi = new BitmapImage(new Uri("ms-appx:///Assets/avatar-placeholder.png"));
            photo.Source = bi;
            photo.Source = await (photo.Source as BitmapImage).AsWriteableBitmapAsync();

            if (selected != null)
            {
                if (selected.Photo != null)
                {
                    photo.Source = await selected.Photo.AsWriteableBitmapAsync();
                    used_placeholder = false;
                }
                guid = selected.ID;
                name.Text = selected.Name;
                phone.Text = selected.Phone.ToString();
                if (selected.Email != null) email.Text = selected.Email;
                else email.Text = "";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private async void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            var ph = used_placeholder ? null : await ((WriteableBitmap)photo.Source).AsByteArrayAsync();
            var pars = new Contractor
            {
                ID = guid,
                Photo = ph,
                Name = name.Text,
                Email = email.Text,
                Phone = long.Parse(phone.Text)
            };
            Frame.Navigate(typeof(MainPage), pars);
        }

        private void uploadBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
