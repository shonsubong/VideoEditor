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
        public MediaClip Clip { get; private set; }

        private MediaComposition composition;

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

        
        public VideoClip(MediaClip clip)
        {
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
            Clip.TrimTimeFromEnd = new TimeSpan(endTick);
            //UpdateBitmapThumbnail();
        }
        
        public async void UpdateBitmapThumbnail()
        {
            BitmapImage bitmap = new BitmapImage();
            
            composition.Clips.Add(Clip);
            var thumbnail = await composition.GetThumbnailAsync(Clip.StartTimeInComposition,
                100, 100, VideoFramePrecision.NearestFrame);
            
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
            randomAccessStream.Seek(0);
            bitmap.SetSource(randomAccessStream);
            composition.Clips.Clear();

            ImgSource = bitmap;           
        }        
    }
}
