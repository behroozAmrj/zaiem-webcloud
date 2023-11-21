using Cloud.IU.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IU.WEB.InfraSructure
{
    public interface IEntity
    {
        string Titel { get; set; }
        string Image { get; set; }
        string Handler { get; set; }
        EntityType Type { get; set; }
        object Result { get; set; }
        List<CustomView> CustomViews {get; set;}
        List<string> LayoutFileList { get; set; }

        object DoWork();

        Boolean Delete();

        string LoadTemplate(string TemplateName);
    }
}
