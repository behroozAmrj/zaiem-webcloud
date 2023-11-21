using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloud.Foundation.Infrastructure
{
    public abstract class FileAndConfig<T> where T : class
    {
        protected string filename;
        public FileAndConfig(string FileName)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + FileName;
            if (File.Exists(path))
            {
                filename = path;
            }
            else
            { throw new Exception(" file is not exist"); }
        }


        public virtual string[] Read()
        { 
            return (File.ReadAllLines(filename));
        }
    }
}