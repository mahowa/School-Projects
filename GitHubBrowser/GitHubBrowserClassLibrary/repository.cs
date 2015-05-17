using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace githubConnect
{
    public class Repository
    {
        public Repository(string username, string repname, string description, string avatar, string email, Languages language, string stars)
        {
            Username = username;
            RepName = repname;
            Description = description;
            languages = language;
            Avatar = avatar;
            Email = email;
            Stars = stars;
        }
      public string Username {get;private set;}
      public string RepName { get; private set; }
      public string Description { get; private set; }
      public Languages languages { get; private set; }
      public string Avatar { get; private set; }
      public string Email { get; set; }
      public string Stars { get; set; }
    }
    
}
