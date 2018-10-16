using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Effects;

namespace UWPDemo.Effect
{
    public class VideoEffectTest : IVideoEffectDefinition
    {
        public string ActivatableClassId => throw new NotImplementedException();

        public IPropertySet Properties => throw new NotImplementedException();
    }
}
