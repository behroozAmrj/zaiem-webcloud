using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloud.IU.WEB.Repository
{
    public class WindowTemplate : FileAndConfig<WindowTemplate>
    {
        private string filename;

        public WindowTemplate(string FileName)
            : base(FileName)
        {

        }

        public HtmlString ReadFileAsHtmlString()
        {

            var windowcontent = File.ReadAllText(base.filename);
            var hs = new HtmlString(windowcontent);
            return (hs);
        }

        
    }





}