using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UWPDemo.ViewModels
{
    public class VideoPreviewViewModel : ViewModelBase
    {
        public VideoManagerVM VideoManger { get; private set; }

        public ICommand RefreshVideoCommand { get; private set; }

        public VideoPreviewViewModel()
        {
            VideoManger = ViewModelDispatcher.VideoManager;
            RefreshVideoCommand = new RelayCommand(() => ExecuteRefreshVideo());
        }

        private async void ExecuteRefreshVideo()
        {
            VideoManger.RefreshPreviewVideo();
        }
    }
}
