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

        public async void AddClip(StorageFile file)
        {
            Clip clip = new Clip(await MediaClip.CreateFromFileAsync(file));

            Clips.Add(clip);
        }

        public async void AddandTrimTickClip(StorageFile file, long startTick, long endTick)
        {
            Clip clip = new Clip(await MediaClip.CreateFromFileAsync(file));
            Clips.Add(clip);
            clip.Trim(startTick, endTick);
            clip.Thumbnail = await clip.MediaClip.GetThumbnailAsync();
        }

        public async void AddandTrimSecClip(Media media, double startSec, double endSec)
        {
            Clip clip = new Clip(media.MediaClip.Clone());
            Clips.Add(clip);
            clip.Trim(startSec, endSec);
            clip.Thumbnail = await clip.MediaClip.GetThumbnailAsync();
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
            }
        }
    }
}
