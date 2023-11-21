using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloud.Foundation.Infrastructure
{
    public abstract class ApplicationAndViewFiles <T>   where T : class
    {
        protected string filename;
        public abstract string Name { set; get; }
        public abstract string Directory { set; get; }



        public ApplicationAndViewFiles(string FileName)
        {

            var path = AppDomain.CurrentDomain.BaseDirectory + FileName;
            if (File.Exists(path))
            {
                filename = path;
            }
            else
            { throw new Exception(" file is not exist"); }

        }

        public ApplicationAndViewFiles()
        {

        }
        public abstract  T Read();
        
    }
}