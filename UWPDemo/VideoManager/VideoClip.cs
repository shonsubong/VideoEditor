using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPDemo.VideoManager
{
    public class VideoClip : ViewModelBase
    {
        private StorageFile file;
        public MediaClip Clip { get; private set; }

        private MediaComposition composition;

        public ImageSource ImgSource { get; set; }

        public long StartTick
        {
            get { return Clip.TrimTimeFromStart.Ticks; }
        }
        public long EndTick
        {
            get { return Clip.TrimTimeFromEnd.Ticks; }
        }

        public long DurationTicks
        {
            get { return Clip.TrimmedDuration.Ticks; }
        }

        
        public VideoClip(StorageFile file)
        {
            this.file = file;
            initalize();            
        }

        private async void initalize()
        {
            Clip = await MediaClip.CreateFromFileAsync(file);
            composition = new MediaComposition();
            composition.Clips.Add(Clip);
        }
        
        public void ClearTrim()
        {
            Trim(0, Clip.OriginalDuration.Ticks);
        }

        public void Trim(long startTick, long endTick)
        {
            Clip.TrimTimeFromStart = new TimeSpan(startTick);
            Clip.TrimTimeFromEnd = new TimeSpan(endTick);

            //ImageSource = GetBitmapThumbnail();
        }


        public async Task<BitmapImage> GetBitmapThumbnail()
        {
            BitmapImage bitmap = new BitmapImage();

            var thumbnail = await GetThumbnailAsync(file);
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
            randomAccessStream.Seek(0);
            bitmap.SetSource(randomAccessStream);

            return bitmap;
        }

        private async Task<IInputStream> GetThumbnailAsync(StorageFile file)
        {
            var mediaComposition = new MediaComposition();
            mediaComposition.Clips.Add(Clip);
            return await mediaComposition.GetThumbnailAsync(Clip.StartTimeInComposition, 
                0, 0, VideoFramePrecision.NearestFrame);
        }

    }
}
