using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPDemo.Util;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace UWPDemo
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public MainPage()
        {
            this.InitializeComponent();

            Current = this;
        }

      
        public void NotifyUser(string strMessage, NotifyType type)
        {
            // If called from the UI thread, then update immediately.
            // Otherwise, schedule a task on the UI thread to perform the update.
            if (Dispatcher.HasThreadAccess)
            {
                UpdateStatus(strMessage, type);
            }
            else
            {
                var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdateStatus(strMessage, type));
            }
        }

        private void UpdateStatus(string strMessage, NotifyType type)
        {
            //switch (type)
            //{
            //    case NotifyType.StatusMessage:
            //        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            //        break;
            //    case NotifyType.ErrorMessage:
            //        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            //        break;
            //}

            //StatusBlock.Text = strMessage;

            //// Collapse the StatusBlock if it has no text to conserve real estate.
            //StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            //if (StatusBlock.Text != String.Empty)
            //{
            //    StatusBorder.Visibility = Visibility.Visible;
            //    StatusPanel.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    StatusBorder.Visibility = Visibility.Collapsed;
            //    StatusPanel.Visibility = Visibility.Collapsed;
            //}

            //// Raise an event if necessary to enable a screen reader to announce the status update.
            //var peer = FrameworkElementAutomationPeer.FromElement(StatusBlock);
            //if (peer != null)
            //{
            //    peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            //}
        }

        private void videoClipsShowButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            VideoSplitView.IsPaneOpen = !VideoSplitView.IsPaneOpen;

            if(VideoSplitView.IsPaneOpen)
            {
                VideoLibraryTitle.Visibility = Visibility.Visible;
                VideoLibraryView.Visibility = Visibility.Visible;
            }
            else
            {
                VideoLibraryTitle.Visibility = Visibility.Collapsed;
                VideoLibraryView.Visibility = Visibility.Collapsed;
            }
                
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            JudgeWidth(e.NewSize.Width);
        }

        private enum WidthEnum
        {
            Initialize,

            PhoneNarrow,
            PhoneStrath,

            Pad,
            Pc
        }
        WidthEnum owe = WidthEnum.Initialize;//OldWidthEnum
        WidthEnum nwe = WidthEnum.Initialize;//NewWidthEnum

        private void JudgeWidth(double w)
        {
            if (w < 600) nwe = WidthEnum.PhoneNarrow;
            else if (w >= 600 && w < 800) nwe = WidthEnum.PhoneStrath;
            else if (w >= 800 && w < 1000) nwe = WidthEnum.Pad;
            else if (w >= 1000) nwe = WidthEnum.Pc;

            if (nwe != owe)
            {

                if (nwe == WidthEnum.PhoneNarrow || nwe == WidthEnum.PhoneStrath)
                {
                    VideoSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                    VideoSplitView.IsPaneOpen = false;

                    ZoomSlider.Visibility = Visibility.Collapsed;
                    SplitButton.Visibility = Visibility.Visible;
                }
                else if (nwe == WidthEnum.Pad || nwe == WidthEnum.Pc)
                {
                    VideoSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                    VideoSplitView.IsPaneOpen = true;

                    ZoomSlider.Visibility = Visibility.Visible;
                    SplitButton.Visibility = Visibility.Collapsed;
                }
                
                owe = nwe;
            }
        }

        private void SplitButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
