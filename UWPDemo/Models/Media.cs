using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPDemo.Models
{
    /// <summary>
    /// 파일로 부터 읽은 오리지널 Media 파일
    /// </summary>
    public class Media : ViewModelBase
    {
        public Media()
        {
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private BitmapImage bitmap;
        public BitmapImage Bitmap
        {
            get { return bitmap; }
            set
            {
                bitmap = value;
                RaisePropertyChanged();
            }
        }

        private SolidColorBrush brush;
        public SolidColorBrush Brush
        {
            get { return brush; }
            set
            {
                brush = value;
                RaisePropertyChanged();
            }
        }

        private MediaClip clip;
        public MediaClip Clip
        {
            get { return clip; }
            set
            {
                clip = value;
                RaisePropertyChanged();
            }
        }
    }
}
