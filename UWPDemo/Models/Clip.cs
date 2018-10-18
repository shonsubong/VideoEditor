using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPDemo.Models
{
    /// <summary>
    /// Story Board 에서 사용하기 위한 Media Clip
    /// </summary>
    public class Clip : Media
    {
        
        public string CaptionText
        {
            get { return CaptionClip.Text; }
            set
            {
                CaptionClip.Text = value;
                RaisePropertyChanged();
            }
        }

        public Caption CaptionClip
        {
            get; set;
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

        private MediaOverlay overlay;
        public MediaOverlay Overlay
        {
            get { return overlay; }
            set
            {
                overlay = value;
                RaisePropertyChanged();
            }
        }

        public Clip(MediaClip mediaClip) : base(mediaClip)
        {
            
        }

        public void SetOverlay(MediaClip clip)
        {
            overlay = new MediaOverlay(clip);
        }

        public static Clip CreateClip(MediaClip mediaClip, string caption = "")
        {
            Clip clip = new Clip(mediaClip.Clone());
            clip.CaptionClip = new Caption(caption, clip.MediaClip.TrimmedDuration);
            return clip;
        }

        public static Clip CreateClip(MediaClip mediaClip, double start, double end, string caption = "")
        {
            Clip clip = new Clip(mediaClip.Clone());
            clip.Trim(start, end);
            clip.CaptionClip = new Caption(caption, clip.MediaClip.TrimmedDuration);
            //clip.AddVideoEffect();
            return clip;
        }

        public static Clip CreateClip(Color color, double start, double end, string caption = "")
        {
            Clip clip = new Clip(MediaClip.CreateFromColor(color, TimeSpan.FromSeconds(end) - TimeSpan.FromSeconds(start)));
            clip.Trim(start, end);
            clip.CaptionClip = new Caption(caption, clip.MediaClip.TrimmedDuration);
            //clip.AddVideoEffect();
            return clip;
        }
    }
}
