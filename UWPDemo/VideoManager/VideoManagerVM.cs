using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Models;
using UWPDemo.Util;
using UWPDemo.VideoManager;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.Transcoding;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWPDemo.ViewModels
{
    public class VideoManagerVM : ViewModelBase
    {
        private MainPage rootPage { get { return MainPage.Current; } }
        
        private StorageItemAccessList storageItemAccessList;
        private StorageFile pickedFile;
        private MediaStreamSource mediaStreamSource;
        private StoryBoard storyBoard;

        private MediaElement previewVideo { get { return rootPage.CurVideoPreview.PreviewVideo; } }
        private MediaComposition composition { get { return storyBoard.Composition; } }

        public Media Drop = null;

        public ObservableCollection<Media> MediaClipList { get; private set; }

        public ObservableCollection<Media> ColorClipList { get; private set; }


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
            storyBoard = new StoryBoard();
            storyBoard.StoryBoardClipsUpdated += storyBoard_StoryBoardClipsUpdated;

            storageItemAccessList = StorageApplicationPermissions.FutureAccessList;
            storageItemAccessList.Clear();

            MediaClipList = new ObservableCollection<Media>();
            ColorClipList = new ObservableCollection<Media>();
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

            //SplitVideoFile();
        }

        private void storyBoard_StoryBoardClipsUpdated(object sender, EventArgs e)
        {
            RefreshPreviewVideo();
        }

        public void RefreshPreviewVideo()
        {
            storyBoard.AppendAllClips();
            UpdateTimelineMarkerCollection();

            previewVideo.Position = TimeSpan.Zero;
            mediaStreamSource = composition.GeneratePreviewMediaStreamSource((int)previewVideo.ActualWidth, (int)previewVideo.ActualHeight);
            if (mediaStreamSource != null)
            {
                previewVideo.SetMediaStreamSource(mediaStreamSource);
            }
        }

        private void UpdateTimelineMarkerCollection()
        {
            previewVideo.Markers.Clear();
            foreach (Clip clip in storyBoard.Clips)
            {
                TimelineMarker marker = new TimelineMarker();
                marker.Time = clip.MediaClip.StartTimeInComposition;
                marker.Text = "marker";
                previewVideo.Markers.Add(marker);
            }
        }
        
        public void SetPreviewVideoPositionTo(Clip clip)
        {
            if(!ChangingSelectItem)
                previewVideo.Position = new TimeSpan(clip.MediaClip.StartTimeInComposition.Ticks + 1);
        }

        public bool ChangingSelectItem = false;
        public void SetStoryBoardSelectItemTo(TimeSpan position)
        {
            ChangingSelectItem = true;
            foreach (Clip clip in storyBoard.Clips)
            {
                if (position > clip.MediaClip.StartTimeInComposition && position <= clip.MediaClip.EndTimeInComposition)
                {
                    rootPage.CurStoryBoardView.CurStoryList.SelectedItem = clip;
                    ChangingSelectItem = false;
                    return;
                }
            }
            ChangingSelectItem = false;
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
            catch (Exception e)
            {

            }

        }

        public void SplitVideoFile(Media media)
        {
            try
            {
                //storyBoard.Clear();

                double step = 1.2;

                //storyBoard.AddClip(media);

                AppendClip(media, 0 * step, 1 * step);

                AppendClip(media, 1 * step, 2 * step);

                AppendClip(media, 2 * step, 3 * step);

                AppendClip(media, 3 * step, 4 * step);

                AppendClip(media, 4 * step, 5 * step);

                AppendClip(media, 5 * step, 6 * step);

            }
            catch (Exception e)
            {

            }
        }

        public void TrimVideo()
        {

        }

        public void AppendClip(Media media, double startSec, double endSec)
        {
            storyBoard.AddClip(media, startSec, endSec);
        }

        public void InsertClip(Media media, double startSec, double endSec, int index)
        {
            storyBoard.AddClip(media, startSec, endSec, index);
        }


    }
}
