using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo.Models
{
    /// <summary>
    /// Story Board 에서 사용하기 위한 Media Clip
    /// </summary>
    public class Clip
    {
        private Media media;
        public Clip(Media media)
        {
            this.media = media;
        }
    }
}
