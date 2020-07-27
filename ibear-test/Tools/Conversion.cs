using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ibear_test.Tools
{
    public static class Conversion
    {
        public async static Task<WriteableBitmap> AsWriteableBitmapAsync(this byte[] bytes)
        {
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var size = new BitmapSize { Width = decoder.PixelWidth, Height = decoder.PixelHeight };
                var wb = new WriteableBitmap((int)size.Width, (int)size.Height);
                await wb.SetSourceAsync(stream);
                return wb;
            }
        }

        public async static Task<WriteableBitmap> AsWriteableBitmapAsync(this BitmapImage bi)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(bi.UriSource);
            using (var stream = await file.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var size = new BitmapSize { Width = decoder.PixelWidth, Height = decoder.PixelHeight };
                var wb = new WriteableBitmap((int)size.Width, (int)size.Height);
                await wb.SetSourceAsync(stream);
                return wb;
            }
        } 

        public async static Task<WriteableBitmap> AsWriteableBitmapAsync(this BitmapImage bi, StorageFile file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var size = new BitmapSize { Width = decoder.PixelWidth, Height = decoder.PixelHeight };
                var wb = new WriteableBitmap((int)size.Width, (int)size.Height);
                await wb.SetSourceAsync(stream);
                return wb;
            }
        }

        public static async Task<byte[]> AsByteArrayAsync(this WriteableBitmap wb)
        {
            using (var stream = new InMemoryRandomAccessStream())
            {
                var result = await stream.ReadAsync(wb.PixelBuffer, wb.PixelBuffer.Length, InputStreamOptions.None);
                return result.ToArray();
            }
        }

    }
}
