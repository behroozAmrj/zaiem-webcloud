using Cloud.Core.GitLabDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class GitLabController : Controller
    {
        //
        // GET: /GitLab/
        public ActionResult GitLab_Load()
        {

            String userID = string.Empty;
            try
            {
                if (Session["userID"] != null)
                    userID = Session["userID"].ToString();
                string pUrl = "http://gitlab-server.com/api/v3/projects?private_token=bmyNz8tYYwheFuL36tow";
                string iUrl = "http://gitlab-server.com/api/v3/issues?private_token=bmyNz8tYYwheFuL36tow";
                var gitLab = new GitLabManagement(userID);
                gitLab.AsyncLoadGitContents(pUrl,
                                            iUrl);
                var projectsList = gitLab.ProjectsList;
                var issueList = gitLab.IssuesList;
                return View();
            }
            catch (Exception ex)
            {
                Log.Tracking.Logging.ErrorlogRegister(ex.Message,
                                                        MethodBase.GetCurrentMethod().GetType(),
                                                        userID);
                return View();
            }
        }
    }
}