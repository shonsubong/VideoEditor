﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo.Models
{
    public class DataItem
    {
        public string Title
        {
            get;
            private set;
        }

        public DataItem(string title)
        {
            Title = title;
        }
    }
}
