using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPDemo.Models;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;

namespace UWPDemo.ViewModels
{
    public class VideoLibraryViewModel : ViewModelBase
    {
        public VideoManagerVM VideoManger { get; private set; }

        ObservableCollection<Media> MediaClipList = new ObservableCollection<Media>();

        public ICommand AddVideoCommand { get; private set; }
        
        public VideoLibraryViewModel()
        {
            VideoManger = ViewModelDispatcher.VideoManager;
            AddVideoCommand = new RelayCommand(() => ExecuteAddVideo());            
        }

        private async void ExecuteAddVideo()
        {
            VideoManger.ImportVideoFileAsync();
        }
    }
}
