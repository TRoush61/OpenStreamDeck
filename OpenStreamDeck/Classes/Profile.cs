using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenStreamDeck.ProfileObjects;
using Newtonsoft.Json;

namespace OpenStreamDeck.ProfileObjects
{
    [JsonObject("Profile")]
    public class Profile
    {
        [JsonProperty("ProfileName")]
        public string ProfileName { get; set; }
        [JsonProperty("Pages")]
        public List<Page> Pages { get; set; }
        [JsonProperty("Path")]
        public string ProfilePath;

        [JsonConstructor]
        public Profile(string profileName)
        {
            var pages = new List<Page>();
            pages.Add(new Page("MainPage"));
            Pages = pages;
            ProfileName = profileName;
            ProfilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/profiles/" + ProfileName + ".json";
        }
    }
}
