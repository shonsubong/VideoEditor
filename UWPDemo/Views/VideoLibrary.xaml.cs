using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPDemo.Models;
using UWPDemo.Util;
using UWPDemo.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace UWPDemo.Views
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class VideoLibrary : Page
    {
        public VideoLibrary()
        {
            this.InitializeComponent();
            this.DataContext = App.VideoManager;

            AddColor(Colors.Black, 0, 1);
            AddColor(Colors.White, 0, 1);
            AddColor(Colors.Gray, 0, 1);
            AddColor(Colors.Red, 0, 1);
            AddColor(Colors.Green, 0, 1);
            AddColor(Colors.Blue, 0, 1);
            
        }

        private void MediaList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            App.VideoManager.Drop = e.Items.First() as Media;
        }

        private void MediaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Media m = MediaList.SelectedItem as Media;
        }

        private bool IsImageFile(StorageFile file)
        {
            if (file.FileType == ".jpg" 
                || file.FileType == ".jpeg"
                || file.FileType == ".png"
                || file.FileType == ".bmp"
                || file.FileType == ".gif"
                || file.FileType == ".tif"
                || file.FileType == ".tiff")
            {
                return true;
            }
            return false;
        }
        private async void AddMediaFileButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;            
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".m4v");
            filePicker.FileTypeFilter.Add(".mov");
            filePicker.FileTypeFilter.Add(".asf");
            filePicker.FileTypeFilter.Add(".avi");
            filePicker.FileTypeFilter.Add(".wmv");

            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".gif");
            filePicker.FileTypeFilter.Add(".tif");
            filePicker.FileTypeFilter.Add(".tiff");

            filePicker.ViewMode = PickerViewMode.Thumbnail;
            
            var files = await filePicker.PickMultipleFilesAsync();

            foreach (StorageFile file in files)
            {
                if (file != null)
                {
                    try
                    {
                        StorageItemAccessList storageItemAccessList = StorageApplicationPermissions.FutureAccessList;
                        storageItemAccessList.Add(file);

                        if(IsImageFile(file))
                        {
                            Media media = new Media(await MediaClip.CreateFromImageFileAsync(file, new TimeSpan(0, 1, 0)));
                            media.Thumbnail = await file.GetThumbnailBitmapAsync();
                            media.Name = file.Name;
                            App.VideoManager.MediaClipList.Add(media);
                        }
                        else
                        {
                            Media media = new Media(await MediaClip.CreateFromFileAsync(file));
                            media.Thumbnail = await file.GetThumbnailBitmapAsync();
                            media.Name = file.Name;
                            App.VideoManager.MediaClipList.Add(media);
                        }
                    }
                    catch (Exception)
                    {
                        App.Tip("file err");
                    }
                }
            }
        }

        private async void AddColor(Color color, double startSec, double endSec)
        {
            Media media = new Media(MediaClip.CreateFromColor(color, TimeSpan.FromSeconds(endSec) - TimeSpan.FromSeconds(startSec)));

            media.Brush = new SolidColorBrush(color);
            media.Thumbnail = await media.MediaClip.GetThumbnailAsync(320, 180);
            App.VideoManager.ColorClipList.Add(media);
        }

        private void ColorList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            App.VideoManager.Drop = e.Items.First() as Media;
        }

        private void ColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Media m = ColorList.SelectedItem as Media;
        }
    }
}
