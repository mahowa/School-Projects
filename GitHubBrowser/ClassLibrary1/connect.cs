using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using githubConnect;

namespace githubConnect
{
    public class Connect
    {
        // You'll need to put your own OAuth token here
        // It needs to have repo deletion capability
        private const string TOKEN = "a5bc197f0f814935ac94636cb76f65b1f23a6728";

        // You'll need to put your own GitHub user name here
        private const string USER_NAME = "willdenms";

        // You'll need to put your own login name here
        private const string EMAIL = "willdenms@gmail.com";

        // You'll need to put one of your public REPOs here
        private const string PUBLIC_REPO = "TestRepo";

        public Connect() { }
        public static HttpClient CreateClient()
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
        /// Prints out the names of the organizations to which the user belongs
        /// </summary> 
        public static async void GetReposAsync()
        {
            Dictionary<string, Repository> repos = new Dictionary<string, Repository>();
            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.GetAsync("/repositories");
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    dynamic orgs = JsonConvert.DeserializeObject(result);
                    foreach (dynamic org in orgs)
                    {
                        Repository temp = new Repository(org.login, org.name, org.description, org.avatar_url, null);
                        repos.Add(org.login, temp);
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
                foreach (var key in repos)
                {
                    using (HttpClient client2 = CreateClient())
                    {
                        HttpResponseMessage response2 = await client2.GetAsync("GET /users/:" + key.Key);
                        if (response.IsSuccessStatusCode)
                        {
                            String result2 = await response2.Content.ReadAsStringAsync();
                            dynamic orgs2 = JsonConvert.DeserializeObject(result2);
                            foreach (dynamic org in orgs2)
                            {
                                repos[key.Key].Email = org.email;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: " + response.StatusCode);
                            Console.WriteLine(response.ReasonPhrase);
                        }
                    }
                }
            }
        }
    }
}
