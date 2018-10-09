using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Util;
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
    public class VideoManagerViewModel : ViewModelBase
    {
        private MainPage rootPage;
        private StorageItemAccessList storageItemAccessList;
        private StorageFile pickedFile;
        private MediaComposition composition;
        private MediaStreamSource mediaStreamSource;
        private MediaElement previewVideo;

        public MediaElement PreviewVideo
        {
            get { return previewVideo; }
            set
            {
                previewVideo = value;
                RaisePropertyChanged();
            }
        }
        

        public VideoManagerViewModel()
        {
            rootPage = MainPage.Current;
            previewVideo = new MediaElement();
            previewVideo.AutoPlay = false;
            previewVideo.AreTransportControlsEnabled = true;

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
                       
            previewVideo.SetSource(await pickedFile.OpenReadAsync(), pickedFile.ContentType);
        }

        public async Task ExportVideoFile()
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

        public void TrimVideo()
        {

        }

        public void AppendClip()
        {

        }

        
    }
}
