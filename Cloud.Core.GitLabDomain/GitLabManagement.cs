using Cloud.Foundation.Infrastructure;
using Cloud.IO.NetworkService;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Core.GitLabDomain
{
    public class GitLabManagement
    {
        private string token;
        public AutoResetEvent projectEvent = new AutoResetEvent(false);
        private AutoResetEvent issueEvent = new AutoResetEvent(false);
        private List<GProject> _projectsList;
        private List<GIssue> _issueList;
        private string userID;
        public List<GProject> ProjectsList
        {
            get
            {
                projectEvent.WaitOne();
                return (_projectsList);
            }
            set { }
        }

        public List<GIssue> IssuesList
        {
            get
            {
                issueEvent.WaitOne();
                return (_issueList);
            }
            set { }
        }
        public GitLabManagement(String userID)
        {
            if (string.IsNullOrEmpty(userID))
                this.userID = "noUserID";
            else
                this.userID = userID;
        }


        public List<GProject> getProjectList()
        {
            projectEvent.WaitOne();
            return (this._projectsList);
        }
        public void AsyncLoadGitContents(string pURL, string iURL)
        {
            //Log.Tracking.Logging.INFOlogRegistrer("loading Async gitlab started ",
            //                                        this.userID,
            //                                        MethodBase.GetCurrentMethod().GetType());
            var TloadProject = new Thread(() =>
            {
                RetrieveGitLabProjects(pURL,
                                        projectEvent);

            });
            var TloadIssue = new Thread(() =>
            {
                RetrieveGitLabIssues(iURL);
            });
            TloadProject.Start();
            TloadIssue.Start();


        }

        private void RetrieveGitLabProjects(string URL, AutoResetEvent projectEven)
        {
            if (string.IsNullOrEmpty(URL))
                throw new Exception("url is null or empty");

            var cloudServer = new CloudServers();
            using (var projectsStream = cloudServer.RetrieveContentNoneHeader(URL) as Stream)
            {
                using (var sReader = new StreamReader(projectsStream))
                {
                    var contentext = sReader.ReadToEnd();
                    var content = JArray.Parse(contentext);
                    var projectsList = content.Select((x) => new GProject()
                    {
                        ID = (int)x["id"],
                        Name = x["name"].ToString(),
                        Desc = x["description"].ToString(),
                        Public = Convert.ToBoolean(x["public"].ToString()),
                        HTTP_URL = x["http_url_to_repo"].ToString(),
                        Web_URL = x["web_url"].ToString(),
                        Name_With_NameSpace = x["name_with_namespace"].ToString(),
                        Created_at = x["created_at"].ToString(),
                        Last_activity = x["last_activity_at"].ToString()
                    }).ToList();

                    sReader.Close();
                    projectEven.Set();
                    this._projectsList = projectsList;
                }
            }
        }

        private void RetrieveGitLabIssues(string URL)
        {
            if (string.IsNullOrEmpty(URL))
                throw new Exception("url is null or empty");

            var cloudServer = new CloudServers();
            using (var issueStream = cloudServer.RetrieveContentNoneHeader(URL) as Stream)
            {
                using (var sReader = new StreamReader(issueStream))
                {
                    var contentext = sReader.ReadToEnd();
                    var content = JArray.Parse(contentext);
                    var issueList = content.Select((x) => new GIssue()
                    {
                        ID = (int)x["id"],
                        Desc = x["description"].ToString(),
                        IID = (int)x["iid"],
                        ProjectID = (int)x["project_id"],
                        Title = x["title"].ToString(),
                        State = x["state"].ToString(),
                        Created_at = x["created_at"].ToString(),
                        Update_at = x["updated_at"].ToString()
                    }).ToList();

                    sReader.Close();
                    issueEvent.Set();
                    this._issueList = issueList;
                }
            }
        }

    }
}
