using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Models;
using UWPDemo.Util;
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
            Composition.OverlayLayers.Add(new MediaOverlayLayer());
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
                Clip clip = Clip.CreateClip(media.MediaClip, startSec, endSec, "merong");
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
                Clip clip = Clip.CreateClip(color, startSec, endSec, "merong");
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

        public void AppendAllClips()
        {
            Composition.Clips.Clear();

            foreach(Clip videoClip in Clips)
            {
                Composition.Clips.Add(videoClip.MediaClip);
                //Composition.OverlayLayers[0].Overlays
            }
        }
    }
}
