using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloud.IO.Repository
{
    public class WindowTemplate : FileAndConfig<WindowTemplate>
    {
        private string filename;

        public WindowTemplate(string FileName)
            : base(FileName)
        {

        }

        public HtmlString ReadFileAsHtmlString(string Height, string Width)
        {

            string windowcontent = File.ReadAllText(base.filename);
            if ((!string.IsNullOrEmpty(Height)) &&
                (!string.IsNullOrEmpty(Width)))
            {
                string width = string.Format("width:{0};",
                                                Width);
                string height = string.Format("height:{0};",
                                                Height);
                windowcontent =  windowcontent.Replace("width:700px;",
                                        width);
                windowcontent = windowcontent.Replace("height:500px;",
                                        height);
            }
            var hs = new HtmlString(windowcontent);
            return (hs);
        }


    }





}