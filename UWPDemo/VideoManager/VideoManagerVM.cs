using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Util;
using UWPDemo.VideoManager;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.Transcoding;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace UWPDemo.ViewModels
{
    public class VideoManagerVM : ViewModelBase
    {
        private MainPage rootPage { get { return MainPage.Current; } }
        private StorageItemAccessList storageItemAccessList;
        private StorageFile pickedFile;
        
        private MediaStreamSource mediaStreamSource;
        private MediaElement previewVideo;
        private StoryBoard storyBoard;
        private MediaComposition composition { get { return storyBoard.Composition; } }

        public MediaElement PreviewVideo
        {
            get { return previewVideo; }
            set
            {
                previewVideo = value;
                RaisePropertyChanged();
            }
        }

        public StoryBoard StoryBord
        {
            get { return storyBoard; }
            set
            {
                storyBoard = value;
                RaisePropertyChanged();
            }
        }


        public VideoManagerVM()
        {            
            previewVideo = new MediaElement();
            previewVideo.AutoPlay = false;
            previewVideo.AreTransportControlsEnabled = true;

            storyBoard = new StoryBoard();
            storyBoard.StoryBoardClipsUpdated += storyBoard_StoryBoardClipsUpdated;

            storageItemAccessList = StorageApplicationPermissions.FutureAccessList;
            storageItemAccessList.Clear();

        }

        public async Task ImportVideoFileAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");
            pickedFile = await picker.PickSingleFileAsync();
            if (pickedFile == null)
            {
                //rootPage.NotifyUser("File picking cancelled", NotifyType.ErrorMessage);
                return;
            }
            storageItemAccessList.Add(pickedFile);

            SplitVideoFile();
        }

        private void storyBoard_StoryBoardClipsUpdated(object sender, EventArgs e)
        {
            RefreshStoryBoard();
            //RefreshPreviewVideo();
        }

        public void RefreshStoryBoard()
        {
            storyBoard.RefreshAllThumbnails();
        }

        public void RefreshPreviewVideo()
        {
            storyBoard.AppendAllClips();

            previewVideo.Position = TimeSpan.Zero;

            mediaStreamSource = composition.GeneratePreviewMediaStreamSource((int)previewVideo.ActualWidth, (int)previewVideo.ActualHeight);
            if(mediaStreamSource != null)
                previewVideo.SetMediaStreamSource(mediaStreamSource);
      
            //previewVideo.SetSource(await pickedFile.OpenReadAsync(), pickedFile.ContentType);
        }

        public async Task ExportVideoFile()
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileSavePicker();
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
                picker.FileTypeChoices.Add("MP4 files", new List<string>() { ".mp4" });
                picker.SuggestedFileName = "TrimmedClip.mp4";

                
                StorageFile file = await picker.PickSaveFileAsync();
                                
                if (file != null && composition != null)
                {
                    var saveOperation = composition.RenderToFileAsync(file, MediaTrimmingPreference.Precise);
                    saveOperation.Progress = new AsyncOperationProgressHandler<TranscodeFailureReason, double>(async (info, progress) =>
                    {
                        await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                        {
                            rootPage.NotifyUser(string.Format("Saving file... Progress: {0:F0}%", progress), NotifyType.StatusMessage);
                        }));
                    });
                    saveOperation.Completed = new AsyncOperationWithProgressCompletedHandler<TranscodeFailureReason, double>(async (info, status) =>
                    {
                        await rootPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                        {
                            try
                            {
                                var results = info.GetResults();
                                if (results != TranscodeFailureReason.None || status != AsyncStatus.Completed)
                                {
                                    rootPage.NotifyUser("Saving was unsuccessful", NotifyType.ErrorMessage);
                                }
                                else
                                {
                                    rootPage.NotifyUser("Trimmed clip saved to file", NotifyType.StatusMessage);
                                }
                            }
                            finally
                            {
                                // Remember to re-enable controls on both success and failure
                                //EnableButtons(true);
                            }
                        }));
                    });
                }
                else
                {
                    rootPage.NotifyUser("User cancelled the file selection", NotifyType.StatusMessage);
                    //EnableButtons(true);
                }
            }
            catch(Exception e)
            {

            }
            
        }

        public void SplitVideoFile()
        {
            try
            {
                storyBoard.Clear();

                storyBoard.AddandTrimSecClip(pickedFile, 0, 1);

                storyBoard.AddandTrimSecClip(pickedFile, 1, 2);

                storyBoard.AddandTrimSecClip(pickedFile, 2, 3);

                storyBoard.AddandTrimSecClip(pickedFile, 3, 4);

                storyBoard.AddandTrimSecClip(pickedFile, 4, 5);


            }
            catch (Exception e)
            {
                
            }
        }

        public void TrimVideo()
        {

        }

        public void AppendClip()
        {

        }

        
    }
}
