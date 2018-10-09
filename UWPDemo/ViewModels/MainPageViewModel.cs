using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.AccessCache;

namespace UWPDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private VideoClipsViewModel videoClipsVM;
        private VideoPreviewViewModel videoPreviewVM;
        private StoryBoardViewModel storyBoardVM;
        private VideoManagerViewModel videoManger;

        public ICommand ExportVideoCommand { get; private set; }


        public MainPageViewModel()
        {
            videoClipsVM = ViewModelDispatcher.VideoClipsViewModel;
            videoPreviewVM = ViewModelDispatcher.VideoPreviewViewModel;
            storyBoardVM = ViewModelDispatcher.StoryBoardViewModel;
            videoManger = ViewModelDispatcher.VideoManager;

            ExportVideoCommand = new RelayCommand(ExecuteExportVideo);
        }

        private async void ExecuteExportVideo()
        {
            await videoManger.ExportVideoFile();
        }
    }
}
