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
        public ICommand ExportVideoCommand { get; private set; }

        public VideoManagerVM VideoManager = App.VideoManager;

        public MainPageViewModel()
        {   
            ExportVideoCommand = new RelayCommand(ExecuteExportVideo);
        }

        private async void ExecuteExportVideo()
        {
            await App.VideoManager.ExportVideoFile();
        }
    }
}
