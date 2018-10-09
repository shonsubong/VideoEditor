using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo.ViewModels
{
    public class StoryBoardViewModel : ViewModelBase
    {
        private VideoManagerViewModel videoManger;

        public StoryBoardViewModel()
        {
            videoManger = ViewModelDispatcher.VideoManager;            
        }
      
    }
}
