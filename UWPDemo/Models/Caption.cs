using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Editing;
using Windows.UI;

namespace UWPDemo.Models
{
    public class Caption
    {
        private readonly CanvasDevice _dev;
        private CanvasDrawingSession _ruler;
        private string _text = "";
        private float _fontSize, _width, _height;
        private TimeSpan _duration;

        public Caption(string text, TimeSpan duration, float fontSize = 16.0f)
        {
            try
            {
                _duration = duration;
                _dev = CanvasDevice.GetSharedDevice();
                _ruler = new CanvasRenderTarget(_dev, 1, 1, 96).CreateDrawingSession();
                _updateText(text, fontSize);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ~Caption()
        {
            _ruler.Dispose();
            _dev.Dispose();
            _rstImg.Dispose();
        }

        public MediaOverlay Overlay
        {
            get; set;
        }

        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value) return;
                _duration = value;
                _makeClip();
            }
        }
        
        public string Text
        {
            get { return _text; }
            set { _updateText(value, FontSize); }
        }

        public float FontSize
        {
            get { return _fontSize; }
            set { _updateText(_text, value); }
        }

        public float Width { get { return _width; } }
        public float Height { get { return _height; } }

        private void _updateText(string text, float fontSize)
        {
            if (_text.Equals(text) && FontSize.Equals(fontSize)) return;
            if (String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text)) return;
            _text = text;
            _fontSize = fontSize;

            // Measure textbox size
            var textLayout = new CanvasTextLayout(_ruler, text, new CanvasTextFormat { FontSize = fontSize }, 0.0f, 0.0f)
            {
                WordWrapping = CanvasWordWrapping.NoWrap
            };
            var textSize = textLayout.LayoutBounds;

            _width = (float)textSize.Width;
            _height = (float)textSize.Height;

            _rstImg = new CanvasRenderTarget(_dev, Width, Height, 96);
            using (var ds = _rstImg.CreateDrawingSession())
            {
                ds.Clear(Colors.Black);
                ds.DrawTextLayout(textLayout, new System.Numerics.Vector2(0.0f, 0.0f), Colors.White);
            }

            _makeClip();
        }

        private CanvasRenderTarget _rstImg;
        private void _makeClip()
        {
            Overlay = new MediaOverlay(MediaClip.CreateFromSurface(_rstImg, Duration));
            Overlay.Opacity = 0.5;
        }
    }
}
