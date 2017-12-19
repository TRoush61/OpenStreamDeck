using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenStreamDeck.ProfileObjects;
using System.IO;

namespace OpenStreamDeck.ConfigManagement
{
    class ProfileManager
    {
        public static Profile loadProfile(string fileLocation)
        {
            if (String.IsNullOrEmpty(fileLocation) || !File.Exists(fileLocation))
            {
                return null;
            }

            JsonSerializer serializer = new JsonSerializer();
            Profile profile;
            using (StreamReader sr = new StreamReader(fileLocation))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    profile = serializer.Deserialize<Profile>(reader);
                }
            }
            return profile;
        }

        public static bool saveProfile(Profile profile)
        {
            var fileLocation = profile.ProfilePath;
            if (String.IsNullOrEmpty(fileLocation) || profile == null)
            {
                return false;
            }

            FileInfo fileInfo = new FileInfo(fileLocation);
            if (!fileInfo.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(fileLocation))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, profile);
                }
            }
            return true;
        }
    }
}
