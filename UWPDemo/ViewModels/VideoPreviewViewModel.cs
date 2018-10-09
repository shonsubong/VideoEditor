using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo.ViewModels
{
    public class VideoPreviewViewModel : ViewModelBase
    {
        public VideoManagerViewModel VideoManger { get; private set; }

        public VideoPreviewViewModel()
        {
            VideoManger = ViewModelDispatcher.VideoManager;
        }
    }
}
