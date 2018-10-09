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
        private MediaComposition composition;

        public StoryBoard()
        {
            StoryBoardClips = new ObservableCollection<VideoClip>();
            composition = new MediaComposition();
        }

        public void AddClip(StorageFile file)
        {
            VideoClip clip = new VideoClip(file);
            StoryBoardClips.Add(clip);
        }

        public void AddandTrimClip(StorageFile file, long startTick, long endTick)
        {
            VideoClip clip = new VideoClip(file);            
            StoryBoardClips.Add(clip);
            clip.Trim(startTick, endTick);
        }

        public void Clear()
        {
            StoryBoardClips.Clear();
        }

        public MediaComposition GetAppendVideo()
        {
            composition.Clips.Clear();

            foreach(VideoClip videoClip in StoryBoardClips)
            {
                composition.Clips.Add(videoClip.Clip);
            }

            return composition;
        }



    }
}
