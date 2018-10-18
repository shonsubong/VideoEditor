using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Core;
using Microsoft.Graphics.Canvas;
using Windows.Graphics.Imaging;

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
                SetSubtitleOverlay(subtitle);
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

        private List<MediaOverlay> overlayList;

        private MediaOverlay subtitleOverlay;
        public MediaOverlay SubtitleOverlay
        {
            get { return subtitleOverlay; }
            set
            {
                subtitleOverlay = value;
                RaisePropertyChanged();
            }
        }

        public Clip(MediaClip mediaClip) : base(mediaClip)
        {
            overlayList = new List<MediaOverlay>();
        }

        public void SetSubtitleOverlay(string subtitile)
       {
            using (CanvasRenderTarget frame = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), 100.0f, 100.0f, 96f))
            {
                using (var session = frame.CreateDrawingSession())
                {
                    session.DrawText(subtitile, new System.Numerics.Vector2(100, 100), Colors.White);
                }
                subtitleOverlay = new MediaOverlay(MediaClip.CreateFromSurface(frame, MediaClip.OriginalDuration));
                subtitleOverlay.Position = new Windows.Foundation.Rect(0, 0, 400, 200);
                subtitleOverlay.Opacity = 0.75;
            }
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
            //clip.AddVideoEffect();
            return clip;
        }

        public static Clip CreateClip(Color color, double start, double end, string subtitle = "")
        {
            Clip clip = new Clip(MediaClip.CreateFromColor(color, TimeSpan.FromSeconds(end) - TimeSpan.FromSeconds(start)));
            clip.Trim(start, end);
            clip.Subtitle = subtitle;
            //clip.AddVideoEffect();
            return clip;
        }
    }
}
