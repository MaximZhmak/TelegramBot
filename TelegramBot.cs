using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using SimpleJSON;


namespace ConsoleApplication9
{
    public class TelegramBot
    {
        private static string api = @"https://api.telegram.org/bot";
        private const string Token = @"";
        private static int LastUpdateID = 0;

        public static void Main()
        {
            Thread thread = new Thread(GetUpdates);
            thread.IsBackground = true;
            thread.Start();
        }

        private static void GetUpdates()
        {
            using (var webClient = new WebClient())
            {
                while (true)
                {
                    Thread.Sleep(500);
                    LastUpdateID++;

                    string response =
                        webClient.DownloadString(api + Token + "/getupdates?offset=" + LastUpdateID.ToString());
                    var N = JSON.Parse(response);
                    foreach (JSONNode r in N["result"].AsArray)
                    {
                        LastUpdateID = r["update_id"].AsInt;
                        string sender = r["message"]["from"]["username"].ToString();
                        string incomingMessage = r["message"]["text"];
                        Console.WriteLine(sender + ": " + incomingMessage);

                        var Answer = AutomaticAnswer(incomingMessage);
                        SendMessage(Answer, r["message"]["chat"]["id"].AsInt);
                    }
                }
            }
        }

        private static string AutomaticAnswer(string incomingMessage)
        {
            switch (incomingMessage)
            {

                default:
                    return "Hello";
            }
        }


        static void SendMessage(string message, int chatId)
        {
            using (var webClient = new WebClient())
            {
                var pars = new NameValueCollection();
                pars.Add("text", message);
                pars.Add("chat_id", chatId.ToString());

                webClient.UploadValues(api + Token + "/sendMessage", pars);
            }
        }
    }
}
