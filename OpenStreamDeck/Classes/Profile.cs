using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenStreamDeck.ProfileObjects;
using Newtonsoft.Json;
using System.IO;

namespace OpenStreamDeck.ProfileObjects
{
    [JsonObject("Profile")]
    public class Profile
    {
        [JsonProperty("ProfileName")]
        public string ProfileName { get; set; }
        [JsonProperty("MainPage")]
        public Page MainPage { get; set; }
        [JsonProperty("Path")]
        public string ProfilePath;
        [JsonIgnore]
        public bool nameChanged;

        [JsonConstructor]
        public Profile()
        {

        }

        public Profile(string profileName)
        {
            MainPage = new Page("MainPage");
            ProfileName = profileName;
            ProfilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/profiles/" + ProfileName + ".json";
            nameChanged = false;
        }

        public void updateProfilePath()
        {
            //removes old profile if one existed
            if (File.Exists(ProfilePath))
            {
                File.Delete(ProfilePath);
            }
            ProfilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/profiles/" + ProfileName + ".json";
        }
    }
}
