using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.Core.Models
{
    // IEntity Extenssion Method Class
    public static class IEntity_EX
    {
        public static void IEntityListJoinToAccessory(this List<IEntity> List, List<LayoutConfigRepository> ConfigList)
        {
            if (List == null)
                return;
            foreach (var item in List)
            {
                try
                {
                    var config = ConfigList.FirstOrDefault(x => x.Name == item.GetType().Name);
                    if (config != null)
                    {
                        var configfile = new ConfigRepository("\\Application-Service\\" + config.Directory);
                        var list = configfile.ReadOne("//appconfig//image");
                        string first = list.First().Image;
                        item.Image = string.Format("/Application-Service/{0}/{1}",
                                                        config.Directory,
                                                        first);
                    }
                }
                catch
                {

                }


            }
        }



        public static List<string> ConfigRepositoryListToStringList(this List<ConfigRepository> ConfigList)
        {
            var outlist = new List<string>();
            foreach (var item in ConfigList)
            {
                outlist.Add(item.Name);
            }

            return (outlist.ToList());
        }


        // Block
        // this extenssion method convert RBSConfigRepository to RunBySelf and  copy those to new list
        //public static void ConvertRBSListToDeskTopItems(this List<RBSConfigRepository> RBSList)
        //{
        //    foreach (var rbs in RBSList)
        //    {
        //        var configfile = new ConfigRepository("\\Application-Service\\" + rbs.Directory);
        //        var configlist = configfile.Read(AppConfigType.application);
        //        string firstimage = configlist.First().Image;
        //        rbs.Image = string.Format("/Application-Service/{0}/{1}",
        //                                            rbs.Directory,
        //                                            firstimage);
        //    }
        //}



        public static void ConvertRBSListToDeskTopItems(this List<RBSConfigRepository> RBSList, List<RunBySelf> RunBySelfList)
        {
            string path = "\\Application-Service\\";
            foreach (var rbs in RBSList)
            {
                try
                {
                    var rbsobj = new RunBySelf();
                    var arbs = new ARBSConfig(path + rbs.Directory + "\\config.xml",
                                              rbs.Directory);
                    var item = arbs.Read();
                    //var list = arbs.APIsList(); // read apis list from config files
                    rbsobj.Handler = item.Handler;
                    rbsobj.Image = string.Format("/Application-Service/{0}/{1}",
                                                        rbs.Directory,
                                                        item.Image);
                    rbsobj.Titel = item.Titel;
                    rbsobj.Type = -1;
                    RunBySelfList.Add(rbsobj);
                }
                catch { }

            }
        }

        public static void ConvertRBSManagementServiceToDesktopItems(this List<RbsApps> RBSList, List<RunBySelf> RunRySelfList)
        {
            string path = "\\Application-Service\\";
            foreach (var rbs in RBSList)
            {
                try
                {
                    var rbsobj = new RunBySelf();

                    //var list = arbs.APIsList(); // read apis list from config files
                    rbsobj.Handler =  rbs.FirstPage;
                    rbsobj.Image = string.Format("/Application-Service/{0}/{1}",
                                                        rbs.source,
                                                        rbs.Image);
                    rbsobj.Titel = rbs.Title;
                    rbsobj.Type = -1;
                    RunRySelfList.Add(rbsobj);
                }
                catch { }

            }

        }
    }



}