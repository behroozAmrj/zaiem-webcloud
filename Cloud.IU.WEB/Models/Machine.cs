using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using Cloud.IU.WEB.InfraSructure;
using CRC_SDK.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class Machine : IEntity, IMachine
    {

        #region private Data Define

        string _title;
        string _image;
        zServer _zservermachine;
        IEntity _entity;
        IMachine _machine;
        EntityType _entitytype;
        string _machinevnc;
        string _handler;
        object _result;
        string _sessionID;
        string _userId;
        List<CustomView> _customviews;
        List<string> layoutfilelist;
        #endregion

        #region Public Data Define
        public string Titel { get { return (this._title); } set { this._title = value; } }

        public string Image { get { return (this._image); } set { this._image = value; } }

        public IEntity IEntity { get { return (_entity); } set { this._entity = value; } }

        public IMachine IMachine { get { return (_machine); } set { this._machine = value; } }
        public string MachineVNC { get { return (this._machinevnc); } set { ;} }
        public EntityType Type { get { return (this._entitytype); } set { this._entitytype = value; } }
        public List<CustomView> CustomViews { get { return (this._customviews); } set { } }
        public string Handler { get { return (this._handler); } set { this._handler = value; } }
        public string SessionID { get { return (this._sessionID); } set { this._sessionID = value; } }
        public List<string> LayoutFileList { get { return (this.layoutfilelist); } set { ; } }
        #endregion

        public Machine(string _Titel, string _Image, zServer _ZServer, EntityType Types)
        {
            if ((!string.IsNullOrEmpty(_Titel)) &&
                (!string.IsNullOrEmpty(_Image)) &&
                (_ZServer != null))
            {
                this._title = _Titel;
                if (!string.IsNullOrEmpty(_Image))
                    this._image = _Image;
                else
                {
                    int iid = (int)Types;
                    this._image = "/images/" + iid.ToString() + ".png";
                }

                this._zservermachine = _ZServer;
                this._handler = _ZServer.ID;

                if (Types == EntityType.Default)
                {
                    this._entitytype = EntityType.Machine;
                }
                else
                { this._entitytype = Types; }
            }
            else
            { throw new Exception("one or more parameres are empty or null"); }
        }

        public Machine(string Handler, string UserID)
        {
            if ((!string.IsNullOrEmpty(Handler)) &&
                (!string.IsNullOrEmpty(UserID)))
            {
                Initialize();
                this.Handler = Handler;
                var zserver = STRepository.StrRepository.RetrievezServer(Handler,
                                                                        UserID);
                this._userId = UserID;
                if (zserver != null)
                    this._zservermachine = zserver;

            }
        }

        public Machine()
        {

        }

        private void Initialize()
        {

        }
        public CRC_SDK.Classes.zServer zServerMachine { get { return (this._zservermachine); } set { this._zservermachine = value; } }


        public object DoWork()
        {
            try
            {
                if (this._zservermachine != null)
                {
                    this._machinevnc = this._zservermachine.GetVNC();
                    this._result = this._machinevnc;
                    var appconfig = new LayoutConfigRepository("Application-Service\\AppsConfig.xml");
                    var layoutconfig = appconfig.ReadOne("//root//layouts//layout[@name='" + this.GetType().Name + "']");
                    var config = new ConfigRepository(ConfigurationManager.AppSettings.Get("appservice") + appconfig.Directory);
                    var layoutlist = config.Read_LayoutList("//appconfig//layouts//layout");
                    layoutfilelist = new List<string>(layoutlist.ConfigRepositoryListToStringList());
                    return (this._result);
                }
                else
                    throw new Exception(" zserver machine is null ");
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format(" can not get vnc due to this error {0} ",
                                                    ex.Message));
            }
        }


        public object Result { get { return (this._result); } set { ; } }


        public bool Delete()
        {
            var result = false;
            try
            {
                if (!string.IsNullOrEmpty(this._sessionID))
                {

                    var crccloud = Models.STRepository.StrRepository.RetrievezStack(SessionID);
                    crccloud.DeleteServer(this._zservermachine);
                    result = true;
                    Models.STRepository.StrRepository.DeletezServerMachine(zServerMachine.ID,
                                                                            this._userId);

                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {

                result = false;
            }

            return (result);
        }





        public string LoadTemplate(string TemplateName)
        {

            if (!string.IsNullOrEmpty(TemplateName))
            {
                var appconfig = new LayoutConfigRepository("Application-Service\\AppsConfig.xml");
                var layoutconfig = appconfig.ReadOne("//root//layouts//layout[@name='" + this.GetType().Name + "']");
                if (layoutconfig == null)
                    return (null);
                var config = new ConfigRepository(ConfigurationManager.AppSettings.Get("appservice") + layoutconfig.Directory);
                var layoutfilename = config.ReadLayoutAddress("//appconfig//layouts//layout[text()='" + TemplateName + "']");
                if (!string.IsNullOrEmpty(layoutfilename))
                {
                    string file = string.Format("{0}\\{1}\\{2}\\{3}\\{4}",
                                                     AppDomain.CurrentDomain.BaseDirectory,
                                                     ConfigurationManager.AppSettings.Get("appservice"),
                                                     layoutconfig.Directory,
                                                     "Layouts",
                                                     layoutfilename);

                    if (config.IsFileExist(file))
                    {
                        string address = string.Format("{0}/{1}/{2}/{3}",
                                                        ConfigurationManager.AppSettings.Get("appservice"),
                                                        layoutconfig.Directory,
                                                        "Layouts",
                                                        layoutfilename);
                        return (address);
                    }
                    else
                    {
                        return (string.Empty);
                    }

                }
                else
                {
                    return (string.Empty);
                }
            }
            else
            { return (string.Empty); }
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
    }
}