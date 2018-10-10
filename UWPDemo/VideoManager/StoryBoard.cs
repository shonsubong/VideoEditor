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

        public ObservableCollection<VideoClip> StoryBoardClips { get; private set; }
        public EventHandler StoryBoardClipsUpdated;

        private MediaComposition composition;
        public MediaComposition Composition
        {
            get
            {
                return composition;
            }
            private set
            {
                composition = value;
            }
        }

        public StoryBoard()
        {
            StoryBoardClips = new ObservableCollection<VideoClip>();
            StoryBoardClips.CollectionChanged += StoryBoardClips_CollectionChanged;
            Composition = new MediaComposition();
        }

        private void StoryBoardClips_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            StoryBoardClipsUpdated?.Invoke(this, null);
        }

        public async void AddClip(StorageFile file)
        {
            VideoClip clip = new VideoClip(await MediaClip.CreateFromFileAsync(file));
            StoryBoardClips.Add(clip);
        }

        public async void AddandTrimClip(StorageFile file, long startTick, long endTick)
        {
            VideoClip clip = new VideoClip(await MediaClip.CreateFromFileAsync(file));            
            StoryBoardClips.Add(clip);
            clip.Trim(startTick, endTick);
        }

        public void Clear()
        {
            StoryBoardClips.Clear();
        }

        public void UpdateStoryBoard()
        {
            composition.Clips.Clear();

            foreach(VideoClip videoClip in StoryBoardClips)
            {
                Composition.Clips.Add(videoClip.Clip);
            }
        }
    }
}
