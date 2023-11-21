using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.Core.Models
{
    public class RunBySelf
    {
        public string Titel { get; set; } // title to display of icon in desktop
        public string Image { get; set; } // image address of the icon in desktop
        public string Handler { get; set; } // address of the first page on running 
        public int Type { get; set; } // it declares type of the desktop items to run in rbs mode
    }
}