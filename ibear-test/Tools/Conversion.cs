using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace ibear_test.Tools
{
    public static class Conversion
    {
        public async static Task<WriteableBitmap> AsWBAsync(this byte[] bytes, int maxWidth, int maxHeight)
        {
            var wb = new WriteableBitmap(maxWidth, maxHeight);
            using (var buffer = wb.PixelBuffer.AsStream())
            {
                await buffer.WriteAsync(bytes, 0, bytes.Length);
                await buffer.FlushAsync();
            }
            return wb;
        }

        public async static Task<(WriteableBitmap, int, int)> AsResizedWBAsync(this StorageFile file, uint width, uint height)
        {
            using (var stream = await file.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                width = width > decoder.PixelWidth ? width : decoder.PixelWidth;
                height = height > decoder.PixelHeight ? height : decoder.PixelHeight;
                var transform = new BitmapTransform() { ScaledWidth = width, ScaledHeight = height, InterpolationMode = BitmapInterpolationMode.Cubic };
                var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                    transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
                var pixels = pixelData.DetachPixelData();

                return (await pixels.AsWBAsync((int)width, (int)height), (int)width, (int)height);
            }
        }

        public static async Task<byte[]> AsByteArrayAsync(this WriteableBitmap wb)
        {
            using (var stream = wb.PixelBuffer.AsStream())
            {
                var buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return buffer;
            }
        }
    }
}
