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


        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private SolidColorBrush brush;
        public SolidColorBrush Brush
        {
            get { return brush; }
            set
            {
                brush = value;
                RaisePropertyChanged();
            }
        }

        private ImageSource imgSource;
        public ImageSource ImgSource
        {
            get
            {
                return imgSource;
            }
            set
            {
                imgSource = value;
                RaisePropertyChanged();
            }
        }

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

        
        public VideoClip(StorageFile file, MediaClip clip)
        {
            this.file = file;
            this.Clip = clip;
            composition = new MediaComposition();           
        }

        public void ClearTrim()
        {
            Trim(0, Clip.OriginalDuration.Ticks);
        }

        public void Trim(long startTick, long endTick)
        {
            Clip.TrimTimeFromStart = new TimeSpan(startTick);
            long trimTickFromEnd = Clip.OriginalDuration.Ticks - endTick;
            Clip.TrimTimeFromEnd = new TimeSpan(trimTickFromEnd);
            //UpdateBitmapThumbnail();
        }

        public void Trim(double startSec, double endSec)
        {
            Clip.TrimTimeFromStart = TimeSpan.FromSeconds(startSec);
            double trimSecondFromEnd = Clip.OriginalDuration.TotalSeconds - endSec;
            Clip.TrimTimeFromEnd = TimeSpan.FromSeconds(trimSecondFromEnd);
            //UpdateBitmapThumbnail();
        }

        public async void UpdateBitmapThumbnail()
        {
            var thumbnail = await GetThumbnailAsync(file);
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
            randomAccessStream.Seek(0);

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(randomAccessStream);            
            ImgSource = bitmap;           
        }

        public async Task<IInputStream> GetThumbnailAsync(StorageFile file)
        {
            var mediaClip = Clip.Clone();
            var mediaComposition = new MediaComposition();
            mediaComposition.Clips.Add(mediaClip);
            return await mediaComposition.GetThumbnailAsync(mediaClip.StartTimeInComposition, 200, 300, VideoFramePrecision.NearestFrame);
        }
    }
}
