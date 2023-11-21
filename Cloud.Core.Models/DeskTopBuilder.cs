using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using Cloud.IU.WEB.InfraSructure;
using Cloud.Log.Tracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Cloud.Core.Models
{
    public class DeskTopBuilder
    {
        public List<LayoutConfigRepository> lcr_List;
        public List<RBSConfigRepository> RBSConfigRepositoryList { get; set; }
        public List<IEntity> dtitems;
        //private delegate void GetLayoutConfigRepository(string XPath , TransparentData Data);
        private string userid;
        public DeskTopBuilder(string UserID)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserID))
                    Models.STRepository.StrRepository.Convert_zServerToMachine(UserID);
                else
                    throw new Exception(" User ID is null or empty ");
            }
            catch (Exception ex)
            {
                throw new Exception(" an error occurd while converting zserver to machine " + ex.Message);
            }
        }

        public void GetDeskTopItemsList(string UserID, ref List<IEntity> DeskTopItemsList, ref List<RunBySelf> RunBySelfList)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(UserID))
                {
                    #region First Thread [Get all entity from zStack (machines)]
                    
                    dtitems = Models.STRepository.StrRepository.RetrieveDesktopItem(UserID);
                    var strrepository = Models.STRepository.StrRepository;
                    var ditemthread = new Thread(new ParameterizedThreadStart(strrepository.Async_RetrieveDesktopItem));

                    ditemthread.Start(new object[]{ this , 
                                                    UserID}); //get machine and other items from repository 
                    #endregion

                    #region Second Thread [Get exist entity config From Repository]

                    var lcr_data = new TransparentData();
                    var lcr = new LayoutConfigRepository("Application-Service\\AppsConfig.xml");
                    var lcrthread = new Thread(new ParameterizedThreadStart(lcr.Async_Read));
                    lcrthread.Start(new object[] { "//root//layouts//layout", 
                                                   lcr_data});  // get desktop Application list from appscondig.xml 
                    //this.lcr_List = lcr.Async_Read("//root//layouts//layout");
                    #endregion

                    #region Thrid [Retrieve all Applications from config file] RBS

                    String xPath = String.Format("//root/applications/application/User[@ID='{0}' or @ID='*']",UserID);
                    var rbs_data = new TransparentData();
                    var rbs = new RBSConfigRepositoryForUsers("Application-Service\\AppsConfig.xml");
                    var rbsThread = new Thread(new ParameterizedThreadStart(rbs.Async_Read));
                    try
                    {
                        rbsThread.Start(new object[]{xPath,
                                                    rbs_data});
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                   
                    Task<List<RbsApps>> rbsList = Task.Factory.StartNew(() => 
                    {

                        var pMessage = new ProxyMessage();
                        pMessage.DeptURL = "webcloud";
                        pMessage.DestinationURL = "RBSManagement";
                        pMessage.Method = string.Format("SDK/GetApps/{0}",
                                                            UserID);
                        pMessage.DateTime = DateTime.Now;
                        pMessage.Content = string.Empty;
                        var apiService = new APIServiceManagement(pMessage ,
                                                                    UserID);
                        var resultStream = apiService.PostBackData();
                        if (string.IsNullOrEmpty(resultStream))
                            return (new List<RbsApps>());
                        try
                        {

                            var desStr = JsonConvert.DeserializeObject(resultStream);
                            var tempist = JsonConvert.DeserializeObject<List<RbsApps>>(desStr.ToString());
                            return (tempist);
                        }
                        catch (Exception)
                        {
                            return (new List<RbsApps>());
                        }
                     });

                    #endregion

                    lcrthread.Join();
                    ditemthread.Join();
                    rbsThread.Join();

                    var list = rbsList.Result;
                    lcr_List = lcr_data.DataValue as List<LayoutConfigRepository>;
                    dtitems.IEntityListJoinToAccessory(lcr_List); // chanage config of by the config of appsconfig.xml 
                    DeskTopItemsList = new List<IEntity>(dtitems);
                    var applicationlist = rbs_data.DataValue as List<RBSConfigRepository>;
                    //applicationlist.ConvertRBSListToDeskTopItems(RunBySelfList);   // set image for RBS application items
                    list.ConvertRBSManagementServiceToDesktopItems(RunBySelfList);
                }
                else
                { 
                    throw new Exception(" GetDeskTopItemsList: User ID in empty "); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<IEntity> GetDeskTopItemsList()
        {
            try
            {
                //Logging.INFOlogRegistrer("get desktop item list",
                //                            this.userid,
                //                            MethodBase.GetCurrentMethod().GetType());
                if (!string.IsNullOrEmpty(userid))
                {
                    var dtitems = Models.STRepository.StrRepository.RetrieveDesktopItem(userid);
                    return (dtitems.ToList());
                }

                else
                { throw new Exception(" GetDeskTopItemsList: User ID in empty "); }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public class RbsApps
    {
        public string Name { get; set; }
        public string source { get; set; }
        public string version { get; set; }
        public Boolean Enable { get; set; }
        public string FirstPage { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Type { get; set; }
    }
}