using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using githubConnect;
using System.Text.RegularExpressions;

namespace githubConnect
{
    /// <summary>
    /// This class acts as the model in our MVC. Connect interacts with the GitHub API, It does this through
    /// setting up an HTTPClient and then by using the "/search/repositories" API provided by GitHub, it
    /// parses the information that is pertinent from the repository, namely: username, language, avatar, description, stars, 
    /// repository name, and the bytes of the language used in that repository.
    /// /// </summary>
    public class Connect
    {
        // You'll need to put your own OAuth token here
        // It needs to have repo deletion capability
        private const string TOKEN = "96c5130840c129fc7af7be4821ea84dd98d1ee9e";

        // You'll need to put your own GitHub user name here
        private const string USER_NAME = "mahowa";

        // You'll need to put your own login name here
        private const string EMAIL = "jdbball1@gmail.com";

        // You'll need to put one of your public REPOs here
        private const string PUBLIC_REPO = "PairWork";
        public Dictionary<string, Repository> repos;
        public string header;
        public int pageNum;

        /// <summary>
        /// Zero args constructor, so it can be implicitly used by the controllor;
        /// </summary>
        public Connect() { }

        /// <summary>
        /// Creates the client for setting up and communicating with the GitHub API.
        /// </summary>
        /// <returns></returns>
        public HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", Uri.EscapeDataString(EMAIL));
            client.DefaultRequestHeaders.Add("Authorization", "token " + TOKEN);
            return client;
        }

        /// <summary>
        /// GetRepoAsync parses through the GitHub repository information. It is capable of searching anything,
        /// provided that the parameter: 'searchterm' is inline with the GitHub API for using "/search/repositories?=". To be more
        /// specific; it is capable of parsing the return of "/search/repositories?=". It creates a Dictionary of unique Repsotories,
        /// (Repository is a class), the keyvalue of that dictionary being a combination of the username and repository name.
        /// It returns an awaitable Task which is the afore mentioned dictionary. For more information on the return item, refer to the
        /// Repository definition
        /// </summary>
        /// <param name="searchterm"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, Repository>> GetReposAsync(string searchterm, CancellationToken cancel)
        {
            if(searchterm == "")
                throw new ArgumentNullException();
            repos = new Dictionary<string, Repository>();                                                       //Dictionary of repositories
            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.GetAsync("/search/repositories?q=" + searchterm);   
                if (response.IsSuccessStatusCode)
                {
                    String login = "", avatar = "", repname = "", description = "", lang = "", bytes = "", starCount = "";
                    Languages language;
                    String result = await response.Content.ReadAsStringAsync();
                    dynamic orgs = JsonConvert.DeserializeObject(result);
                    foreach (dynamic c in orgs.items)
                    {                                       //Parse
                        repname = c.name;               //Repository name
                        login = c.owner.login;          //Username
                        avatar = c.owner.avatar_url;    //avatar url
                        description = c.description;    //repo description
                        lang = c.language;              //main repo Language
                        bytes = c.size;                 //repo size
                        starCount = c.watchers_count;   //Get stars
                        if (String.IsNullOrWhiteSpace(lang))
                            lang = "Unknown";           //Unknown repo language
                        language = new Languages(lang, bytes);
                        //Creates repository object\\
                        Repository temp = new Repository(login, repname, description, avatar, null, language, starCount);      
                        repos.Add(login + repname, temp);                 //KEY to DICTIONARY -> LOGIN + REPNAME
                        cancel.ThrowIfCancellationRequested();            //Allows cancelation of task
                    }

                    //The only reason this would fail is if the link doesn't exist (most likely because there is only one page)
                    try
                    {
                        header = response.Headers.GetValues("Link").FirstOrDefault();   //Get link header
                        pageSift(header);                                               //Get page number (GITHUB API MAXIMUM '34')
                    }
                    catch (Exception) { pageNum = 1;}
                }
                else
                    throw new Exception("Connection Failed ");
                return repos;
            }
        }

        /// <summary>
        /// A simple helper method to facilitate in pagination. Searches for the 
        /// page number provided by the link header, then populates the instance variable pagenum.
        /// </summary>
        /// <param name="link"></param>
        public void pageSift(string link)
        {
            string pattern = @"(\d+)";
            foreach (Match match in Regex.Matches(link, pattern))
                pageNum = Convert.ToInt16(match.Value);
        }
    }
}
