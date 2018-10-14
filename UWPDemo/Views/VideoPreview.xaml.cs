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
        private MediaElement previewVideo;

        public VideoPreview()
        {
            this.InitializeComponent();
            this.DataContext = App.VideoManager;
            this.previewVideo = App.VideoManager.PreviewVideo;
            this.previewVideo.SeekCompleted += PreviewVideo_SeekCompleted;
        }

        private void PreviewVideo_SeekCompleted(object sender, RoutedEventArgs e)
        {
            App.VideoManager.SetStoryBoardSelectItemTo(previewVideo.Position);
        }
    
        private void VideoRefreshButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.VideoManager.RefreshPreviewVideo();
        }
    }
}
