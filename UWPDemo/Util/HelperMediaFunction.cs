using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPDemo.Util
{
    public static class HelperMediaFunction
    {
        public static async Task<BitmapImage> GetThumbnailBitmapAsync(this StorageFile file)
        {
            using (StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.VideosView))
            {
                BitmapImage bitmapImage = new BitmapImage();
                if (thumbnail != null)
                {
                    bitmapImage.SetSource(thumbnail);
                }
                return bitmapImage;
            }
        }

        public static async Task<BitmapImage> GetThumbnailAsync(this MediaClip media, int width = 200, int height = 200)
        {
            var mediaClip = media.Clone();
            var mediaComposition = new MediaComposition();
            mediaComposition.Clips.Add(mediaClip);

            using (ImageStream thumbnail = await mediaComposition.GetThumbnailAsync(mediaClip.StartTimeInComposition, width, height, VideoFramePrecision.NearestFrame))
            {
                using (InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream())
                {
                    await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
                    randomAccessStream.Seek(0);
                    BitmapImage bitmapImage = new BitmapImage();
                    if (thumbnail != null)
                    {
                        bitmapImage.SetSource(randomAccessStream);
                    }
                    return bitmapImage;
                }
            }
        }
    }
}
