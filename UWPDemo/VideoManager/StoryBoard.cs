using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.Storage;

namespace UWPDemo.VideoManager
{
    public class StoryBoard : ViewModelBase
    {
        public ObservableCollection<VideoClip> Clips { get; private set; }

        public MediaComposition Composition = new MediaComposition();

        public EventHandler StoryBoardClipsUpdated;

        public StoryBoard()
        {
            Clips = new ObservableCollection<VideoClip>();
            Clips.CollectionChanged += StoryBoardClips_CollectionChanged;
        }

        private void StoryBoardClips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            StoryBoardClipsUpdated?.Invoke(this, null);
        }

        public async void AddClip(StorageFile file)
        {
            VideoClip clip = new VideoClip(file, await MediaClip.CreateFromFileAsync(file));
            Clips.Add(clip);
        }

        public async void AddandTrimTickClip(StorageFile file, long startTick, long endTick)
        {
            VideoClip clip = new VideoClip(file, await MediaClip.CreateFromFileAsync(file));            
            Clips.Add(clip);
            clip.Trim(startTick, endTick);
        }

        public async void AddandTrimSecClip(StorageFile file, double startSec, double endSec)
        {
            VideoClip clip = new VideoClip(file, await MediaClip.CreateFromFileAsync(file));
            Clips.Add(clip);
            clip.Trim(startSec, endSec);
        }

        public void Clear()
        {
            Clips.Clear();
        }

        public void AppendAllClips()
        {
            Composition.Clips.Clear();

            foreach(VideoClip videoClip in Clips)
            {
                Composition.Clips.Add(videoClip.Clip);
            }
        }

        public void RefreshAllThumbnails()
        {
            foreach (VideoClip videoClip in Clips)
            {
                videoClip.UpdateBitmapThumbnail();
            }
        }
    }
}
