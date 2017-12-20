using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenStreamDeck.ProfileObjects;
using System.IO;
using System.Windows.Forms;

namespace OpenStreamDeck.ConfigManagement
{
    class ProfileManager
    {
        public static Profile loadProfile(string fileLocation)
        {
            if (String.IsNullOrEmpty(fileLocation) || !File.Exists(fileLocation))
            {
                throw new FileNotFoundException();
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

        //TODO: Maybe rework how profiles are saved and overwritten
        public static bool saveProfile(Profile profile)
        {
            if (profile.nameChanged)
            {
                profile.updateProfilePath();
            }
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
            else if(profile.nameChanged)
            {
                var dialogResult = MessageBox.Show("A profile with this name already exists! Would you like to overwrite it?", "Overwrite Profile", MessageBoxButtons.YesNo);
                if (dialogResult.Equals(DialogResult.No))
                {
                    profile.nameChanged = false;
                    return false;
                }
                profile.nameChanged = false;
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
