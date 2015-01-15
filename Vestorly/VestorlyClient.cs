using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using RestSharp;


namespace Vestorly
{
    /// <summary>
    /// Vestorly API Class 
    /// Simply create a new class just type new VestorlyClient('YOUR_API_KEY')
    /// </summary>
    public class VestorlyClient
    {
        private string apiServer;
        private string apiKey;
        private string vestorlyAuthToken;
        private string advisorId;
        private string username;
        private string siteUrl;

        private RestClient client;

        public VestorlyClient(string apiKey, string apiServer = "https://api.vestorly.com")
        {
            this.apiKey     = apiKey;
            this.apiServer  = apiServer;
            this.client     = new RestClient(this.ServerUrl);
        }
        /// <summary>
        /// Starts a connection to the vestorly server and confirms username / password, which it exchanges for a token
        /// </summary>
        /// <param name="username">Vestorly username should be an email address</param>
        /// <param name="password">Vestorly password</param>
        /// <returns></returns>
        public bool Login(string username, string password) 
        {
            RestRequest request = new RestRequest("api/v1/session_management/sign_in", Method.POST);

            this.username = username;

            request.AddParameter("username", username);
            request.AddParameter("password", password);

            var content = client.Execute<LoginResponse>(request).Data;

            if (content != null && !String.IsNullOrWhiteSpace(content.advisor_id))
            {
                this.advisorId = content.advisor_id;
                this.vestorlyAuthToken = content.vestorly_auth;
                this.siteUrl = content.theme.site_url;

                return true;
            }
            else if (content != null)
            {
                throw new VestorlyException(content.message);
            }
            else
            {
                throw new VestorlyException();
            }
        }


        /// <summary>
        /// Starts a connection to the vestorly server using a long running Vestorly token 
        /// </summary>
        /// <param name="token">a vestorly auth token </param>
        /// <returns></returns>
        public bool LoginWithToken(string authToken)
        {
            RestRequest request = new RestRequest("api/v1/session_management/find_by_auth_cookie.json?vestorly-auth=" + authToken, Method.GET);
            try
            {
                var content = client.Execute<LoginResponse>(request).Data;

                if (content != null && !String.IsNullOrWhiteSpace(content.advisor_id))
                {
                    this.advisorId = content.advisor_id;
                    this.vestorlyAuthToken = authToken;
                    this.siteUrl = content.theme.site_url;
                    this.username = content.user.username;
                    return true;
                }
            }
            catch (Exception) { }

            return false; 
        }

        /// <summary>
        /// Creates a new advisor account and activates the account if advisor.active is set to true
        /// </summary>
        /// <param name="advisor">A fullly flushed out advisor object, advisor.password is optional</param>
        /// <returns></returns>
        /// <exception cref="VestorlyException">throws exception on server errors</exception>
        /// <exception cref="VestorlyDuplicateUsernameException">throws exception on duplicate emails</exception>
        /// <exception cref="VestorlyInvalidEmailException">On invalid email addresses</exception>
        public bool CreateAdvisor(Advisor advisor)
        {

            RestRequest request = new RestRequest("api/v1/advisors?api_key=" + this.apiKey, Method.POST);
            request.RequestFormat = DataFormat.Json;

            var advisorContainer = new AdvisorContainer { advisor = advisor };

            request.AddBody(advisorContainer);

            RestResponse response = (RestResponse) client.Execute(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                char[] delimiterChars = { '"', ':', '.'};
                if (!String.IsNullOrWhiteSpace(response.Content)) {
                    var strs = response.Content.Split(delimiterChars);
                    if (strs.Length >= 5 && strs[5] == " Username Email Address Taken")
                    {
                        throw new VestorlyDuplicateUsernameException(strs[4] + strs[5]);
                    }

                    if (strs.Length >= 5 && strs[5] == " Username This field must be an email address")
                    {
                        throw new VestorlyInvalidEmailException(strs[4] + strs[5]);
                    }

                }
                throw new VestorlyException(response.Content);
            }

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
            {
                throw new VestorlyException(response.Content);
            }

            return true;
        }

        /// <summary>
        /// Creates a Post and optionally uploads a PDF file
        /// </summary>
        /// <param name="post">Includes Title, Description and other data about post to upload</param>
        /// <param name="filePath">Optional PDF File to Upload to Vestorly</param>
        /// <param name="shouldPublish">Optional Indicates whether the Post should be published or simply uploaded in a draft state</param>/// 
        /// <returns></returns>
        public bool CreatePost(Post post, String filePath = null, bool shouldPublish = true )
        {
            RestRequest request;

            if (!String.IsNullOrEmpty(filePath))
            {
                UploadFile(post, filePath);
            }

            if (shouldPublish)
            {
                this.AddGroupsToPost(post);
            }

            request = new RestRequest("api/v2/posts", Method.POST);

            request.AddHeader("X-Vestorly-Auth", AuthToken);
            request.RequestFormat = DataFormat.Json;

            var postContainer = new PostContainer { post = post };

            request.AddBody(postContainer);

            RestResponse response = (RestResponse)client.Execute(request);

            return true;
        }


        /// <summary>
        /// Creates a Member and optionally links to a group
        /// </summary>
        /// <param name="member">Includes Email, First Name, Last Name and other data about user to upload</param>
        /// <param name="group">Group which the member may be in</param>
        /// <returns></returns>
        public Member CreateMember(Member member, Group group = null)
        {
            RestRequest request;

            request = new RestRequest("api/v1/advisors/"+this.advisorId+"/members", Method.POST);

            request.AddHeader("X-Vestorly-Auth", AuthToken);
            request.RequestFormat = DataFormat.Json;

            if (group != null) 
            {
                member.group_id = group.id;
            }

            var memberContainer = new MemberContainer { member = member };

            request.AddBody(memberContainer);

            var content = client.Execute<MemberContainer>(request).Data;

            return content == null ? null : content.member;
        }


        /// <summary>
        /// Update a Member and optionally links to a group
        /// </summary>
        /// <param name="member">Includes Email, First Name, Last Name and other data about user to upload</param>
        /// <param name="group">Group which the member may be in</param>
        /// <returns></returns>
        public Member UpdateMember(Member member, Group group = null)
        {
            RestRequest request;

            request = new RestRequest("api/v1/advisors/" + this.advisorId + "/members/"+ member.id, Method.PUT);

            request.AddHeader("X-Vestorly-Auth", AuthToken);
            request.RequestFormat = DataFormat.Json;

            if (group != null)
            {
                member.group_id = group.id;
            }

            var memberContainer = new MemberContainer { member = member };

            request.AddBody(memberContainer);

            var content = client.Execute<MemberContainer>(request).Data;

            return content == null ? null : content.member;
        }


        /// <summary>
        /// Removes a Member from Vestorly
        /// </summary>
        /// <param name="member">Require id of member</param>
        /// <returns></returns>
        public bool DeleteMember(Member member)
        {
            RestRequest request;

            request = new RestRequest("api/v1/advisors/" + this.advisorId + "/members/" + member.id, Method.DELETE);

            request.AddHeader("X-Vestorly-Auth", AuthToken);
            request.RequestFormat = DataFormat.Json;

            var memberContainer = new MemberContainer { member = member };

            request.AddBody(memberContainer);

            RestResponse response = (RestResponse)client.Execute(request);

            return true;
        }

        /// <summary>
        /// Gets a full list of members 
        /// </summary>
        public List<Member> Members
        {
            get
            {
                RestRequest request = new RestRequest("api/v1/advisors/" + this.advisorId + "/members", Method.GET);

                request.AddHeader("X-Vestorly-Auth", AuthToken);
                request.RequestFormat = DataFormat.Json;

                var content = client.Execute<MembersContainer>(request).Data;

                return content.members;
            }
        }


        /// <summary>
        /// The Publisher URL to navigate to Vestorly with, contains single sign on credentials
        /// </summary>
        public string PublisherUrl
        {
            get
            {
                return BuildSSO("/publisher/content/publish");
            }
        }

        /// <summary>
        /// The Publisher Custom Content URL to navigate to Vestorly with, contains single sign on credentials
        /// </summary>
        public string PublisherCustomContentUrl
        {
            get
            {
                return BuildSSO("/publisher/content/my-posts");
            }
        }


        /// <summary>
        /// The Reporting URL to navigate to Vestorly with, contains single sign on credentials
        /// </summary>
        public string ReportingUrl
        {
            get
            {
                return BuildSSO("/publisher/reporting/members-report");
            }
        }

        /// <summary>
        /// The Manage URL to navigate to Vestorly with, contains single sign on credentials
        /// </summary>
        public string ManageUrl
        {
            get
            {
                return BuildSSO("/publisher/content/manage/feed");
            }
        }


        /// <summary>
        /// Vestorly Long running Auth token which can be saved or persisted 
        /// </summary>
        public string AuthToken
        {
            get
            {
                return this.vestorlyAuthToken;
            }
        }


        private void UploadFile(Post post, String filePath) 
        {
            RestRequest request = new RestRequest("api/v1/utilities/file_upload", Method.POST);

            String fileName = Path.GetFileName(filePath);               

            request.AddFile("file", filePath);
            request.AddParameter("file", fileName);
            request.AddHeader("X-Vestorly-Auth", AuthToken);

                
            var content = client.Execute<Dictionary<string, string>>(request).Data;

            if (content == null)
            {
                return;
            }

            string key = content["key"];

            request = new RestRequest("api/v2/posts", Method.POST);

            post.external_url = key;
            post.external_url_source = "custom-content";
            post.external_url_type = "pdf";
        }


        private Group[] GetGroups()
        {
            List<Group> groupList = new List<Group>();

            RestRequest request = new RestRequest("api/v2/groups", Method.GET);

            request.AddHeader("X-Vestorly-Auth", AuthToken);
            request.RequestFormat = DataFormat.Json;

            var content = client.Execute<GroupContainer>(request).Data;

            return content.groups.ToArray();
        }

        private void AddGroupsToPost(Post post)
        {
            if (post.group_ids == null)
            {
                Group[] groups = this.GetGroups();
                List<String> groupIds = new List<String>();
                foreach (Group group in groups)
                {
                    groupIds.Add(group.id);
                }

                post.group_ids = groupIds.ToArray();
            }
        }


        private string BuildSSO(string path) {
            if (String.IsNullOrWhiteSpace(this.siteUrl))
            {
                return "";
            }

            String uriString = this.siteUrl + path + "?email=" + Uri.EscapeDataString(this.username) + "&vestorly_auth=" + this.vestorlyAuthToken;

            UriBuilder uriBuilder = new UriBuilder(uriString);
            uriBuilder.Scheme = "https";
            uriBuilder.Port = 443;

            return uriBuilder.ToString(); 
        }

        private string ServerUrl
        {
            get
            {
                return this.apiServer;
            }
        }
      
    }
}

