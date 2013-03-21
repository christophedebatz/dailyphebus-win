using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace DailyPhebus
{
    class ServerData
    {

        private static string serverUrl = "http://phebus.debatz.fr/start.php?access=camchou";

        public ServerData() { }

        public string sendAndLoad (string queryString)
        {
            WebRequest objWebRequest = System.Net.HttpWebRequest.Create(serverUrl + "&" + queryString);
            WebResponse objWebResponse = objWebRequest.GetResponse();
            StreamReader objStreamReader = null;
            string data = null;

            try
            {
                objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                data = objStreamReader.ReadToEnd();
            }
            catch { }
            finally
            {
                if (objWebResponse != null)
                    objWebResponse.Close();
            }

            return data;
        }

    }
}
