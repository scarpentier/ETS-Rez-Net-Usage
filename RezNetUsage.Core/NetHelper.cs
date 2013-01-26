using System;
using System.Net;
using System.Text;

namespace RezNetUsage.Core
{
	public class NetHelper
	{
        public static string DownloadHtml(string url)
		{
			try
			{
				var webClient = new WebClient();
				var myDatabuffer = webClient.DownloadData(url);
				return Encoding.Default.GetString(myDatabuffer);
			}
			catch (Exception ex)
			{
				throw new Exception("Error while downloading HTML: " + ex.Message);
			}
		}

        public static string DownloadHtml(string url, string username, string password)
        {
            try
            {
                var webClient = new WebClient();

                // Credentials management
                var cache = new CredentialCache {
                    {
                        new Uri(url),
                        "Basic",
                        new NetworkCredential(username, password)
                    }};
                webClient.Credentials = cache;

                var myDatabuffer = webClient.DownloadData(url);
                return Encoding.Default.GetString(myDatabuffer);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while downloading HTML: " + ex.Message);
            }
        }

		public static void DownloadFile(string url, string fileName)
		{
			var webClient = new WebClient();
			webClient.DownloadFile(url, fileName);
		}

        public static bool DoesUrlExists(string url)
        {
            var urlExists = false;
            try
            {
                var req = WebRequest.Create(url);
                var response = (HttpWebResponse)req.GetResponse();
                urlExists = true;
            }
            catch (WebException ex)
            {

            }
            return urlExists;
        }


	}
}
