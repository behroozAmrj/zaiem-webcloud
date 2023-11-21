using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using Cloud.IU.WEB.InfraSructure;
using CRC_SDK.Classes;
using net.openstack.Core.Domain;
using net.openstack.Providers.Zstack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace Cloud.Core.Models
{
    public class Machine : IEntity, IMachine, IDisposable
    {

        #region private Data Define

        string _title;
        string _image;
        Server _zservermachine;
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
        ManualResetEvent mutualEvent = null;
        string height;
        string width;
        String UserID;


        #endregion

        #region Public Data Define
        public string Titel { get { return (this._title); } set { this._title = value; } }

        public string Image { get { return (this._image); } set { this._image = value; } }

        public IEntity IEntity { get { return (_entity); } set { this._entity = value; } }

        // public IMachine IMachine { get { return (_machine); } set { this._machine = value; } }
        public string MachineVNC { get { return (this._machinevnc); } set { ;} }
        public EntityType Type { get { return (this._entitytype); } set { this._entitytype = value; } }
        public List<CustomView> CustomViews { get { return (this._customviews); } set { } }
        public string Handler { get { return (this._handler); } set { this._handler = value; } }
        public string SessionID { get { return (this._sessionID); } set { this._sessionID = value; } }
        public List<string> LayoutFileList { get { return (this.layoutfilelist); } set { ; } }
        public string Width
        {
            get { return width; }
            set { width = value; }
        }

        public string Height
        {
            get { return height; }
            set { height = value; }
        }
        #endregion

        public Machine(String UserID , string _Titel, string _Image, Server _ZServer, EntityType Types)
        {
            if ((!string.IsNullOrEmpty(_Titel)) &&
                (!string.IsNullOrEmpty(_Image)) &&
                (!string.IsNullOrEmpty(UserID)) &&
                (_ZServer != null))
            {
                this._userId = UserID;
                this._title = _Titel;
                if (!string.IsNullOrEmpty(_Image))
                    this._image = _Image;
                else
                {
                    int iid = (int)Types;
                    this._image = "/images/_" + iid.ToString() + "_.png";
                }

                this._zservermachine = _ZServer;
                this._handler = _ZServer.Id;

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
            this.mutualEvent = new ManualResetEvent(false);
        }

        private void Initialize()
        {

        }
        public Server zServerMachine { get { return (this._zservermachine); } set { this._zservermachine = value; } }


        public object DoWork()
        {
            try
            {
                if (this._zservermachine != null)
                {
                    //Log.Tracking.Logging.INFOlogRegistrer("run Machine",
                    //                                        this._userId,
                    //                                        MethodBase.GetCurrentMethod().DeclaringType);
                    this._machinevnc = this._zservermachine.GetVncConsole(ConsoleType.NoVNC);// GetVNC();
                    this._result = this._machinevnc;
                    var appconfig = new LayoutConfigRepository("Application-Service\\AppsConfig.xml");
                    var layoutconfig = appconfig.ReadOne("//root//layouts//layout[@name='" + this.GetType().Name + "']");
                    using (var config = new ConfigRepository("Application-Service\\" + appconfig.Directory))
                    {
                        var layoutlist = config.Read_LayoutList("//appconfig//layouts//layout");
                        layoutfilelist = new List<string>(layoutlist.ConfigRepositoryListToStringList());
                        this.width = config.Width;
                        this.height = config.Height;
                        return (this._result);
                    }
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
                    //crccloud.DeleteServer(this._zservermachine);
                    if (this._zservermachine.Delete())
                    {
                        result = true;
                        Models.STRepository.StrRepository.DeletezServerMachine(zServerMachine.Id,
                                                                                this._userId);
                    }

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
                var config = new ConfigRepository("Application-Service\\" + layoutconfig.Directory);
                var layoutfilename = config.ReadLayoutAddress("//appconfig//layouts//layout[text()='" + TemplateName + "']");
                if (!string.IsNullOrEmpty(layoutfilename))
                {
                    string file = string.Format("{0}\\{1}\\{2}\\{3}\\{4}",
                                                     AppDomain.CurrentDomain.BaseDirectory,
                                                     "Application-Service\\",
                                                     layoutconfig.Directory,
                                                     "Layouts",
                                                     layoutfilename);

                    if (config.IsFileExist(file))
                    {
                        string address = string.Format("{0}/{1}/{2}/{3}",
                                                        "Application-Service\\",
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


        public void Insert(String UserID, String machineName, String imageID, String flavorID, String network)
        {

            String userID = UserID;
            var repService = new RepositoryService.RepositoryClient();
            var userCredential = repService.getUser(userID) ;// Cloud.Core.Models.STRepository.StrRepository.getUserCredential(userID);
            if (userCredential == null)
                throw new Exception("use is not exist in webcloud. <a href=\"/Security/NewLogin?erc=user loggedOut\"> please login again</a>");

            CloudIdentity identity = new CloudIdentity();
            identity.Username = userCredential.UserName;
            identity.Password = userCredential.Password;
            identity.TenantName = userCredential.UserTenant;

            List<string> networkId = new List<string>();
            new CloudNetworksProvider().ListNetworks("", identity).ToList().ForEach(cn =>
            {
                if (cn.Label.Equals(network, StringComparison.InvariantCultureIgnoreCase))
                    networkId.Add(cn.Id);
            });

            var serverProvider = new CloudServersProvider();
            var newServer = serverProvider.CreateServer(machineName, imageID, flavorID, null, null, null, false, false, networkId, null, identity);
            newServer.WaitForActive(600, null, i =>
            {

            });

            var cloudIdentity = new CloudIdentity();
            cloudIdentity.Username = userCredential.UserName;
            cloudIdentity.Password = userCredential.Password;
            cloudIdentity.TenantName = userCredential.UserTenant;
            var serversList = serverProvider.ListServersWithDetails(cloudIdentity).ToList();
            var flavorsList = serverProvider.ListFlavorsWithDetails(null, null, null, null, null, cloudIdentity).ToList();
            var imagesList = serverProvider.ListImagesWithDetails(null, null, null, null, null, null, null, null, cloudIdentity).ToList();
            Cloud.Core.Models.STRepository.StrRepository.FillupServer(serversList,
                                                                                 userID);

        }

        public List<object> retrieveMachine(String UserID, Boolean machineStuff)
        {
            var repService = new RepositoryService.RepositoryClient();
            var repository = Cloud.Core.Models.STRepository.StrRepository;
            var serverList = repository.GetZserverList(UserID);
            var resultList = new List<object>();
            resultList.Add(serverList);
            if (machineStuff)
            {
                resultList.Add(repository.GetZimageList(UserID).ToList());
                resultList.Add(repository.GetZFlavorList(UserID).ToList());
                var identity = new CloudIdentity();
                identity.Username = repService.getUser(UserID).UserName; //repository.getUserCredential(UserID).UserName;
                identity.Password = repService.getUser(UserID).Password;//repository.getUserCredential(UserID).Password;
                identity.TenantName = repService.getUser(UserID).UserTenant;//repository.getUserCredential(UserID).UserTenant;
                List<string> networkId = new List<string>();
                new CloudNetworksProvider().ListNetworks("", identity).ToList().ForEach(cn =>
                {
                    networkId.Add(cn.Label);
                });
                resultList.Add(networkId);
            }
            return (resultList);
        }


        public void machineOperation(String UserID, String machineID, String operation)
        {
            var repService = new RepositoryService.RepositoryClient();

            String userID = UserID;
            //UserProSecurity userCredential = Cloud.Core.Models.STRepository.StrRepository.getUserCredential(userID);
            //if (userCredential == null)
            //    throw new Exception("use is not exist in webcloud. <a href=\"/Security/NewLogin?erc=user loggedOut\"> please login again</a>");
            //UserProSecurity userCred = Cloud.Core.Models.STRepository.StrRepository.getUserCredential(userID);
            CloudIdentity identity = new CloudIdentity();
            identity.Username = repService.getUser(UserID).UserName; //userCred.UserName;
            identity.Password = repService.getUser(UserID).Password; //userCred.Password;
            identity.TenantName = repService.getUser(UserID).UserTenant; //userCred.UserTenant;

            var serverProvider = new CloudServersProvider();
            var machineState = serverProvider.ListServersWithDetails(identity).First(x => x.Id == machineID).Status.Name;

            

            switch (operation.ToLower())
            {
                case "poweron":
                    {
                        serverProvider.StartServer(machineID,
                                                    null,
                                                    identity);
                        serverProvider.WaitForServerState(machineID,
                                                            ServerState.Active,
                                                             new[] { ServerState.Error, ServerState.Unknown, ServerState.Suspended },
                                                            10000,
                                                            identity: identity);
                        break;
                    }
                case "stop":
                    {
                        serverProvider.StopServer(machineID,
                                                    null,
                                                    identity);
                        serverProvider.WaitForServerState(machineID,
                                                            ServerState.ShutOff,
                                                             new[] { ServerState.Error, ServerState.Unknown, ServerState.Suspended },
                                                            10000,
                                                            identity: identity);
                        break;
                    }
                case "restart":
                    {
                        serverProvider.RebootServer(machineID,
                                                        RebootType.Soft,
                                                        null,
                                                        identity);
                        serverProvider.WaitForServerState(machineID,
                                                            ServerState.Active,
                                                            new[] { ServerState.Error, ServerState.Unknown, ServerState.Suspended },
                                                            10000,
                                                            identity: identity);
                        break;
                    }
                case "delete":
                    {

                        serverProvider.DeleteServer(machineID,
                                                    null,
                                                    identity);
                        serverProvider.WaitForServerDeleted(machineID,
                                                            identity: identity);
                        break;
                    }
                default: break;
            }

            var serversList = serverProvider.ListServersWithDetails(identity).ToList();
            Cloud.Core.Models.STRepository.StrRepository.FillupServer(serversList,
                                                                              userID);
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}