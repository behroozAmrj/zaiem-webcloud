using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class DeskTopBuilder
    {
        public List<LayoutConfigRepository> lcr_List;
        public List<RBSConfigRepository> RBSConfigRepositoryList { get; set; }
        public List<IEntity> dtitems;
        private delegate void GetLayoutConfigRepository(List<LayoutConfigRepository> LayoutList);
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

        //Old Load  desktop Entity
        //public List<IEntity> GetDeskTopItemsList(string UserID)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(UserID))
        //        {
        //            dtitems = Models.STRepository.StrRepository.RetrieveDesktopItem(UserID);
        //            var strrepository = Models.STRepository.StrRepository;
        //            var ditemthread = new Thread(new ParameterizedThreadStart(strrepository.Async_RetrieveDesktopItem));

        //            ditemthread.Start(new object[]{ this , 
        //                                            UserID}); //get machine and other items from repository 
        //            var lcr = new LayoutConfigRepository(this,
        //                                                    "Application-Service\\AppsConfig.xml");

        //            var lcrthread = new Thread(new ParameterizedThreadStart(lcr.Async_Read));
        //            lcrthread.Start(new object[] { lcr_List , 
        //                                                "//root//applications//application"});  // get desktop items list from appscondig.xml 

        //            // RBS application are retrieve in this lines
        //            // begin 
        //            var rbs = new RBSConfigRepository(this,
        //                                                "Application-Service\\AppsConfig.xml");
        //            var applicationlist = rbs.Read(AppConfigType.application);
        //            //end

        //            lcrthread.Join();
        //            ditemthread.Join();
        //            lcr_List = null;
        //            dtitems.IEntityListJoinToAccessory(lcr_List); // chanage config of by the config of appsconfig.xml 
        //            applicationlist.ConvertRBSListToDeskTopItems();   // set image for RBS application items
        //            this.RBSConfigRepositoryList = applicationlist;

        //            return (dtitems.ToList());
        //        }

        //        else
        //        { throw new Exception(" GetDeskTopItemsList: User ID in empty "); }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("redirect to login page");

        //    }
        //}


        public void GetDeskTopItemsList(string UserID, ref List<IEntity> DeskTopItemsList, ref List<RunBySelf> RunBySelfList)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserID))
                {
                    dtitems = Models.STRepository.StrRepository.RetrieveDesktopItem(UserID);
                    var strrepository = Models.STRepository.StrRepository;
                    var ditemthread = new Thread(new ParameterizedThreadStart(strrepository.Async_RetrieveDesktopItem));

                    ditemthread.Start(new object[]{ this , 
                                                    UserID}); //get machine and other items from repository 
                    //var lcr = new LayoutConfigRepository(this,
                    //                                        "Application-Service\\AppsConfig.xml");
                    
                    
                    //var lcrthread = new Thread(new ParameterizedThreadStart(lcr.Async_Read));
                    //lcrthread.Start(new object[] { lcr_List , 
                    //                                    "//root//applications//application"});  // get desktop Application list from appscondig.xml 

                    // RBS application are retrieve in this lines
                    // begin 
                    var rbs = new RBSConfigRepository("Application-Service\\AppsConfig.xml");
                    var applicationlist = rbs.ReadOne("//root//applications/application");
                    
                    //end

                    //lcrthread.Join();
                    ditemthread.Join();
                    dtitems.IEntityListJoinToAccessory(lcr_List); // chanage config of by the config of appsconfig.xml 
                    DeskTopItemsList = new List<IEntity>(dtitems);
                    applicationlist.ConvertRBSListToDeskTopItems(RunBySelfList);   // set image for RBS application items
                }
                else
                { throw new Exception(" GetDeskTopItemsList: User ID in empty "); }
            }
            catch (Exception ex)
            {
                throw new Exception("redirect to login page");
            }
        }

        public List<IEntity> GetDeskTopItemsList()
        {
            try
            {
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
}