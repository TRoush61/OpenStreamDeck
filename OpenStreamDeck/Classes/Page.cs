using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenStreamDeck.ProfileObjects;
using Newtonsoft.Json;

namespace OpenStreamDeck.ProfileObjects
{
    [JsonObject("Page")]
    public class Page
    {
        [JsonProperty("Keys")]
        public List<Key> Keys { get; set; }
        [JsonProperty("PageName")]
        public string PageName { get; set; }

        
        public Page(string pageName)
        {
            var keys = new List<Key>();
            for (var i = 0; i < 15; i++)
            {
                keys.Add(new Key());
            }
            Keys = keys;
            PageName = pageName;
        }

        [JsonConstructor]
        private Page()
        {
            Keys = new List<Key>();
            PageName = "NewPage";
        }
    }
}
