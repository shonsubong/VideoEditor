using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo.Models
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
