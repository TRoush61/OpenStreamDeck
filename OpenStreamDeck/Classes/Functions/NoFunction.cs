using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenStreamDeck.Functions;
using OpenStreamDeck.Handler;

namespace OpenStreamDeck.Functions
{
    class NoFunction : KeyFunction
    {
        [JsonConstructor]
        public NoFunction()
        {

        }

        public NoFunction(DeckHandler dh = null) : base(dh)
        {

        }

        public override string getFunctionName()
        {
            return "No Function";
        }

        public override void Run(DeckHandler dh)
        {
            return;
        }

        public override void ShowForm()
        {
            MessageBox.Show("No settings to change for this function");
        }
    }
}
