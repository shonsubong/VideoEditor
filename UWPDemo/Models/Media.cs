﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Util;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.Effects;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPDemo.Models
{
    /// <summary>
    /// 파일로 부터 읽은 오리지널 Media 파일
    /// </summary>
    public class Media : ViewModelBase, ICloneable
    {
        public Media(MediaClip mediaClip)
        {
            this.MediaClip = mediaClip;
        }

        private MediaClipType clipType;
        public MediaClipType ClipType
        {
            get { return clipType; }
            set
            {
                clipType = value;
                RaisePropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private BitmapImage thumbnail;
        public BitmapImage Thumbnail
        {
            get { return thumbnail; }
            set
            {
                thumbnail = value;
                RaisePropertyChanged();
            }
        }

        private SolidColorBrush brush;
        public SolidColorBrush Brush
        {
            get { return brush; }
            set
            {
                brush = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBrushColor
        {
            get { return brush != null; }
        }

        public double DurationSec
        {
            get { return mediaClip.TrimmedDuration.TotalSeconds; }           
        }

       

        private MediaClip mediaClip;
        public MediaClip MediaClip
        {
            get { return mediaClip; }
            set
            {
                mediaClip = value;
                RaisePropertyChanged();
            }
        }

        private TimelineMarker marker;
        public TimelineMarker Marker
        {
            get { return marker; }
            set
            {
                marker = value;
                RaisePropertyChanged();
            }
        }



        public object Clone()
        {
            Media clone = new Media(MediaClip.Clone());
            clone.Brush = Brush;
            clone.Thumbnail = Thumbnail;
            clone.Name = Name;
            return clone;            
        }

        public void ClearTrim()
        {
            Trim(0, mediaClip.OriginalDuration.Ticks);
        }

        public void Trim(long startTick, long endTick)
        {
            mediaClip.TrimTimeFromStart = new TimeSpan(startTick);
            long trimTickFromEnd = mediaClip.OriginalDuration.Ticks - endTick;
            mediaClip.TrimTimeFromEnd = new TimeSpan(trimTickFromEnd);
            //UpdateBitmapThumbnail();
            RaisePropertyChanged("DurationSec");
        }

        public void Trim(double startSec, double endSec)
        {
            Trim(TimeSpan.FromSeconds(startSec).Ticks, TimeSpan.FromSeconds(endSec).Ticks);
        }

        public void AddVideoEffect()
        {
            //clip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(GrayscaleVideoEffect).FullName));
            VideoStabilizationEffectDefinition videoEffect = new VideoStabilizationEffectDefinition();
            //mediaClip.VideoEffectDefinitions.Add(videoEffect);
        }
    }
}
