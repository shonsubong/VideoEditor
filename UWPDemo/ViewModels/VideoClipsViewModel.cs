using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;

namespace UWPDemo.ViewModels
{
    public class VideoClipsViewModel : ViewModelBase
    {
        private VideoManagerVM videoManger;

        public ICommand AddVideoCommand { get; private set; }
        
        public VideoClipsViewModel()
        {
            videoManger = ViewModelDispatcher.VideoManager;
            AddVideoCommand = new RelayCommand(() => ExecuteAddVideo());            
        }

        private async void ExecuteAddVideo()
        {
            await videoManger.ImportVideoFileAsync();
        }
    }
}
