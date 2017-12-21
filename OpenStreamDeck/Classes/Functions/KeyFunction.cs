using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenStreamDeck.Handler;

namespace OpenStreamDeck.Functions
{
    //Base class for Key Functions. Build overrides for additional functions
    public class KeyFunction
    {
        [JsonProperty("isNavigationKey")]
        public bool isNavigationKey = false;

        [JsonConstructor]
        public KeyFunction()
        {

        }

        public KeyFunction(DeckHandler dh)
        {
        }

        public virtual string getFunctionName()
        {
            return null;
        }

        public virtual void Run(DeckHandler dh)
        {
            //override this to implement your function
            return;
        }

        public virtual void ShowForm()
        {
            //override and display your form to collect any data your function needs
        }
    }
}
