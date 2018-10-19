using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPDemo.Models;
using UWPDemo.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace UWPDemo.Views
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class StoryBoardView : Page
    {

        public GridView CurStoryList { get { return StoryList; } }
        public StoryBoardView()
        {
            this.InitializeComponent();
            this.DataContext = App.VideoManager;
            StoryList.ShowsScrollingPlaceholders = true;
        }

        private void StoryList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {

        }

        private void StoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clip clip = StoryList.SelectedItem as Clip;
            if(clip != null)
            {
                App.VideoManager.SetPreviewVideoPositionTo(clip);
                StoryList.ScrollIntoView(StoryList.SelectedItem);
            }
        }

        private void StoryList_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            StoryList.SelectedItem = args.Items.First() as Clip;
        }

        private void StoryList_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.IsGlyphVisible = false;
            e.DragUIOverride.Caption = "Add";
        }

        private void StoryList_Drop(object sender, DragEventArgs e)
        {
            DragOperationDeferral def = e.GetDeferral();
            def.Complete();

            if (App.VideoManager.Drop.MediaClip != null)
            {
                App.VideoManager.SplitVideoFile(App.VideoManager.Drop);
            }
        }

        private void DeleteClipButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Button btn = sender as Button;
            if(btn != null)
            {
                Clip clip = btn.DataContext as Clip;
                if (clip != null)
                {
                    App.VideoManager.StoryBoard.Remove(clip);
                }
            }
        }

        private void ClipItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            try
            {                
                Storyboard sb = ((Grid)sender).Resources["EnterStoryboard"] as Storyboard;
                sb.Begin();
            }
            catch
            {

            }
        }

        private void ClipItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                Storyboard sb = ((Grid)sender).Resources["ExitStoryboard"] as Storyboard;
                sb.Begin();
            }
            catch
            {

            }            
        }

        private void LeftScroll_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetScrollViewer(StoryList);
            scrollViewer.ChangeView(scrollViewer.HorizontalOffset + (-100 * 2), scrollViewer.VerticalOffset, null, false);
        }

        private void LeftScroll_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetScrollViewer(StoryList);
            scrollViewer.ChangeView(scrollViewer.HorizontalOffset + (-100 * 2), scrollViewer.VerticalOffset, null, false);
        }

        private void RightScroll_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetScrollViewer(StoryList);
            scrollViewer.ChangeView(scrollViewer.HorizontalOffset + (100 * 2), scrollViewer.VerticalOffset, null, false);
        }

        public ScrollViewer GetScrollViewer(DependencyObject element)
        {
            if (element is ScrollViewer)
            {
                return (ScrollViewer)element;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }

        
    }
}
