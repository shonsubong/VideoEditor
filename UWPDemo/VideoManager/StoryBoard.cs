using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Models;
using UWPDemo.Util;
using Windows.Foundation;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace UWPDemo.VideoManager
{
    public class StoryBoard : ViewModelBase
    {
        public ObservableCollection<Clip> Clips { get; private set; }

        public MediaComposition Composition { get; private set; }


        public EventHandler StoryBoardClipsUpdated;

        public StoryBoard()
        {
            Composition = new MediaComposition();
            Clips = new ObservableCollection<Clip>();
            Clips.CollectionChanged += StoryBoardClips_CollectionChanged;
        }

        private void StoryBoardClips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            StoryBoardClipsUpdated?.Invoke(this, null);
        }

        public async void AddClip(Media media)
        {
            Clip clip = Clip.CreateClip(media.MediaClip);
            Clips.Add(clip);
            clip.Thumbnail = await clip.MediaClip.GetThumbnailAsync(320, 180);            
        }
               
        public async void AddandTrimSecClip(Media media, double startSec, double endSec)
        {
            long start = TimeSpan.FromSeconds(startSec).Ticks;
            long end = TimeSpan.FromSeconds(endSec).Ticks;
            long duration = media.MediaClip.OriginalDuration.Ticks;

            if (start < end && duration > end)
            {
                Clip clip = Clip.CreateClip(media.MediaClip, startSec, endSec, "this is a color card~");
                Clips.Add(clip);
                clip.Thumbnail = await clip.MediaClip.GetThumbnailAsync(320, 180);
            }
        }

        public async void AddandTrimSecClip(Color color, double startSec, double endSec)
        {
            long start = TimeSpan.FromSeconds(startSec).Ticks;
            long end = TimeSpan.FromSeconds(endSec).Ticks;

            if (start < end)
            {
                Clip clip = Clip.CreateClip(color, startSec, endSec, "this is a video clip!!!");
                Clips.Add(clip);
                clip.Thumbnail = await clip.MediaClip.GetThumbnailAsync(320, 180);
            }
        }

        public void Remove(Clip clip)
        {
            Clips.Remove(clip);
        }

        public void Clear()
        {
            Clips.Clear();
        }

        public void AppendAllClips(double screenWidth, double screenHeight)
        {
            Composition.Clips.Clear();
            Composition.OverlayLayers.Clear();
            var overlayLayer = new MediaOverlayLayer();

            foreach (Clip videoClip in Clips)
            {
                Composition.Clips.Add(videoClip.MediaClip);
                try
                {
                    Rect overlayPosition;
                    overlayPosition.Height = videoClip.CaptionClip.Height;
                    overlayPosition.Width = videoClip.CaptionClip.Width;
                    overlayPosition.X = (screenWidth - overlayPosition.Width) / 2;
                    overlayPosition.Y = screenHeight * 4 / 5;
                    videoClip.CaptionClip.Overlay.Position = overlayPosition;

                    videoClip.CaptionClip.Overlay.Delay = TimeSpan.FromSeconds(0); // this may cause problems -> should be fixed!

                    overlayLayer.Overlays.Add(videoClip.CaptionClip.Overlay);
                    Composition.OverlayLayers.Add(overlayLayer);
                    //Composition.OverlayLayers[0].Overlays
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }
}
