using System;
using System.Net;
using System.Text;

namespace SPACEBAR
{
	public class Net
	{
        public static string DownloadHTML(string url)
		{
			try
			{
				WebClient webClient = new WebClient();
				byte[] myDatabuffer = webClient.DownloadData(url);
				return Encoding.Default.GetString(myDatabuffer);
			}
			catch (Exception ex)
			{
				throw new Exception("Error while downloading HTML: " + ex.Message);
			}
		}

        public static string DownloadHTML(string url, string username, string password)
        {
            try
            {
                WebClient webClient = new WebClient();

                // Credentials management
                CredentialCache cache = new CredentialCache();
                cache.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
                webClient.Credentials = cache;

                byte[] myDatabuffer = webClient.DownloadData(url);
                return Encoding.Default.GetString(myDatabuffer);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while downloading HTML: " + ex.Message);
            }
        }

		public static void DownloadFile(string url, string fileName)
		{
			WebClient webClient = new WebClient();
			webClient.DownloadFile(url, fileName);
		}

        public static bool DoesUrlExists(string url)
        {
            bool urlExists = false;
            try
            {
                WebRequest req = WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                urlExists = true;
            }
            catch (System.Net.WebException ex)
            {

            }
            return urlExists;
        }


	}
}
