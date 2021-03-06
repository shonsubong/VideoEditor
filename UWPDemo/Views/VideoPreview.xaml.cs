﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Storage;
using Windows.Media.Editing;
using Windows.Media.Transcoding;
using Windows.UI.Core;
using Windows.Storage.AccessCache;
using Windows.Media.Core;
using UWPDemo.ViewModels;


// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace UWPDemo.Views
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class VideoPreview : Page
    {
        public MediaElement PreviewVideo { get { return mediaElement; } }

        public VideoPreview()
        {
            this.InitializeComponent();
            this.DataContext = App.VideoManager;
            this.PreviewVideo.AutoPlay = false;
            this.PreviewVideo.AreTransportControlsEnabled = true;
            this.PreviewVideo.SeekCompleted += PreviewVideo_SeekCompleted;
            this.PreviewVideo.RateChanged += PreviewVideo_RateChanged;
            this.PreviewVideo.MarkerReached += PreviewVideo_MarkerReached;
            //this.previewVideo.TransportControls
        }

        private void PreviewVideo_MarkerReached(object sender, TimelineMarkerRoutedEventArgs e)
        {
            App.VideoManager.SetStoryBoardSelectItemTo(e.Marker.Time + new TimeSpan(1));
        }

        private void PreviewVideo_RateChanged(object sender, RateChangedRoutedEventArgs e)
        {
            //App.VideoManager.SetStoryBoardSelectItemTo(previewVideo.Position);
        }

        private void PreviewVideo_SeekCompleted(object sender, RoutedEventArgs e)
        {
            App.VideoManager.SetStoryBoardSelectItemTo(PreviewVideo.Position);
        }
    
        private void VideoRefreshButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.VideoManager.RefreshPreviewVideo();
        }
    }
}
