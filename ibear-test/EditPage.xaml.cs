using ibear_test.Database;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ibear_test.Tools
{
    public sealed partial class EditPage : Page
    {
        private bool used_placeholder = true;
        private Guid guid = Guid.Empty;
        private int width = 0;
        private int height = 0;

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

            if (selected != null)
            {
                if (selected.Photo != null)
                {
                    photo.Source = await selected.Photo.AsWBAsync(selected.Width, selected.Height);
                    used_placeholder = false;
                }
                guid = selected.ID;
                name.Text = selected.Name;
                phone.Text = selected.Phone.ToString();
                width = selected.Width;
                height = selected.Height;
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
            if (name.Text != "" && phone.Text != "")
            {
                var ph = used_placeholder ? null : await ((WriteableBitmap)photo.Source).AsByteArrayAsync();
                var nm = phone.Text.Replace("-", "").Replace("+", "");
                var pars = new Contractor
                {
                    ID = guid,
                    Photo = ph,
                    Name = name.Text,
                    Email = email.Text,
                    Phone = long.Parse(nm),
                    Width = width,
                    Height = height
                };
                Frame.Navigate(typeof(MainPage), pars);
            }
        }

        private async void uploadBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.ComputerFolder;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var res = await file.AsResizedWBAsync(300, 300);
                photo.Source = res.Item1;
                width = res.Item2;
                height = res.Item3;
                used_placeholder = false;
            }
        }
    }
}
