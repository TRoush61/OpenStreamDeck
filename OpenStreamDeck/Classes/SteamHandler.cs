using System;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

// Dexter's API is: E3A07F610E03043190620D392816E1BD

namespace OpenStreamDeck.Classes
{
    class SteamAccess
    {
        static void Main()
        {
            List<string> games = RecentGames("DeftKoh");
        }

        private static List<string> RecentGames(string userName)
        {
            
            //Pulls the website with recent games.
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            string url = "http://steamcommunity.com/id/" + userName + "/games/";
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string raw = reader.ReadToEnd();
            // Do things with the HTML.
            string[] rawRecentGames;
            string splitString = "var rgGames = ";
            rawRecentGames = raw.Split(splitString);
            Console.WriteLine(rawRecentGames);
            return rawRecentGames;
        }

               
    }
    class ParseHTML
    {
        public ParseHTML() { }
        private string ReturnString;

        public string doParsing(string html)
        {
            Thread t = new Thread(TParseMain);
            t.ApartmentState = ApartmentState.STA;
            t.Start((object)html);
            t.Join();
            return ReturnString;
        }

        private void TParseMain(object html)
        {
            WebBrowser wbc = new WebBrowser();
            wbc.DocumentText = "feces of a dummy";        //;magic words        
            HtmlDocument doc = wbc.Document.OpenNew(true);
            doc.Write((string)html);
            this.ReturnString = doc.Body.InnerHtml + " do here something";
            return;
        }
    }
}
