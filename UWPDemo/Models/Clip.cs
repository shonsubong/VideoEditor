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
using System.Drawing;

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

        public Caption CaptionClip { get; private set; }

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

        public MediaOverlay CaptionOverlay
        {
            get { return CaptionClip.Overlay; }            
        }

        public Clip(MediaClip mediaClip) : base(mediaClip)
        {
            overlayList = new List<MediaOverlay>();
        }

        public static Clip CreateClip(MediaClip mediaClip, string caption = "")
        {
            Clip clip = new Clip(mediaClip.Clone());
            clip.CaptionClip = new Caption(caption, clip.MediaClip.TrimmedDuration);
            clip.CaptionText = caption;
            return clip;
        }

        public static Clip CreateClip(MediaClip mediaClip, double start, double end, string caption = "")
        {
            Clip clip = new Clip(mediaClip.Clone());
            clip.Trim(start, end);
            clip.CaptionClip = new Caption(caption, clip.MediaClip.TrimmedDuration);
            clip.CaptionText = clip.CaptionClip.Text;
            //clip.AddVideoEffect();
            return clip;
        }

        //public static Clip CreateClip(Color color, double start, double end, string caption = "")
        //{
        //    Clip clip = new Clip(MediaClip.CreateFromColor(color, TimeSpan.FromSeconds(end) - TimeSpan.FromSeconds(start)));
        //    clip.Trim(start, end);
        //    clip.CaptionClip = new Caption(caption, clip.MediaClip.TrimmedDuration);
        //    //clip.AddVideoEffect();
        //    return clip;
        //}

        public void SetCaption(Size screenSize, double startSec)
        {

        }

        //public void SetCaptionOverlay(string subtitile)
        //{
        //    using (CanvasRenderTarget frame = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), 100.0f, 100.0f, 96f))
        //    {
        //        using (var session = frame.CreateDrawingSession())
        //        {
        //            session.DrawText(subtitile, new System.Numerics.Vector2(100, 100), Colors.White);
        //        }
        //        captionOverlay = new MediaOverlay(MediaClip.CreateFromSurface(frame, MediaClip.OriginalDuration));
        //        captionOverlay.Position = new Windows.Foundation.Rect(0, 0, 400, 200);
        //        captionOverlay.Opacity = 0.75;
        //    }
        //}
    }
}
