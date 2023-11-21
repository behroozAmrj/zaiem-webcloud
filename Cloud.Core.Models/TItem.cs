using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.Core.Models
{
    public class TItem : IEntity
    {
        #region data field
        string title;
        string image;
        string handler;
        EntityType entity;
        List<CustomView> _customviews;
        List<string> layoutfilelist;
        #endregion
        public TItem(string Title)
        {
            title = Titel;
        }
        public TItem()
        {

        }
        public string Titel { get { return (title); } set { title = value; } }
        public string Image { get { return (image); } set { image = value; } }
        public string Handler { get { return (handler); } set { handler = value; } }
        public EntityType Type { get { return (entity); } set { entity = value; } }
        public object Result { get { return (new object()); } set { throw new NotImplementedException(); } }
        public List<string> LayoutFileList { get { return (this.layoutfilelist); } set { this.layoutfilelist = value; } }
        public object DoWork()
        {
            return (null);
        }
        public bool Delete()
        {
            return (true);
        }
        public List<CustomView> CustomViews { get { return (this._customviews); } set { ;} }

        public string LoadTemplate(string TemplateName)
        {
            throw new NotImplementedException();
        }


        Foundation.Infrastructure.EntityType IEntity.Type
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        List<Foundation.Infrastructure.CustomView> IEntity.CustomViews
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public string Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Width
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}