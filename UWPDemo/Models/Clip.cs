using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPDemo.Models
{
    /// <summary>
    /// Story Board 에서 사용하기 위한 Media Clip
    /// </summary>
    public class Clip : Media
    {

        private string subtitle;
        public string Subtitle
        {
            get { return subtitle; }
            set
            {
                subtitle = value;
                RaisePropertyChanged();
            }
        }

        public long TrimStart
        {
            get { return MediaClip.TrimTimeFromStart.Ticks; }
        }

        public long TrimEnd
        {
            get { return MediaClip.TrimTimeFromEnd.Ticks; }
        }

        public long TrimDuration
        {
            get { return MediaClip.TrimmedDuration.Ticks; }
        }

        public Clip(MediaClip mediaClip) : base(mediaClip)
        {
        }

        public static Clip CreateClip(MediaClip mediaClip, string subtitle = "")
        {
            Clip clip = new Clip(mediaClip.Clone());            
            clip.Subtitle = subtitle;
            return clip;
        }

        public static Clip CreateClip(MediaClip mediaClip, double start, double end, string subtitle = "")
        {
            Clip clip = new Clip(mediaClip.Clone());
            clip.Trim(start, end);
            clip.Subtitle = subtitle;
            return clip;
        }
    }
}
