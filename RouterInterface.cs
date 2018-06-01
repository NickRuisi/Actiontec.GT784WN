using System; 
using System.Net;
using System.IO;
using HtmlAgilityPack; 


namespace Actiontec.GT784WN
{
    public class RouterInterface
    {
        #region Private Variables 

        private string _routerAddress;
        private string _routerUsername;
        private string _routerPassword;

        private string _uriTemplate = "http://{0}{1}";

        private CookieContainer _cookieContainer = new CookieContainer();

        #endregion Private Variables

        public RouterInterface(string address, string username, string password)
        {
            this._routerAddress = address;
            this._routerUsername = username;
            this._routerPassword = password;
        }

        public WANStatus GetWANStatus()
        {
            MemoryStream ms = GetRequestData(String.Format(_uriTemplate, _routerAddress, Constants.WANStatusUri));
            HtmlDocument dom = new HtmlDocument();
            dom.Load(ms);
            ms.Seek(0, SeekOrigin.Begin);
            string scriptData = GetScriptData(ms);
            return new WANStatus(dom, scriptData); 
        }

        /// <summary>
        /// Used to request the data exposed on the "Modem Utilization" admin page 
        /// </summary>
        /// <returns>A ModemUtilization data structure containing requested data</returns>
        public ModemUtilization GetModemUtilization()
        {
            MemoryStream ms = GetRequestData(String.Format(_uriTemplate, _routerAddress, Constants.ModemUtilizationUri));
            return new ModemUtilization(GetScriptData(ms));
           
        }

        /// <summary>
        /// Used to request the data exposed on the "Modem Status" page 
        /// </summary>
        /// <returns>A ModemStatus data structure containing requested data</returns>
        public ModemStatus GetStatus()
        {
            //return new ModemStatus(RequestScriptData(String.Format(_uriTemplate, _routerAddress, Constants.ModemStatusUri)));
            MemoryStream ms = GetRequestData(String.Format(_uriTemplate, _routerAddress, Constants.ModemStatusUri));
            return new ModemStatus(GetScriptData(ms)); 
        }

        /// <summary>
        /// Gets content from a URI
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestMethod"></param>
        /// <returns></returns>
        private MemoryStream GetRequestData(string requestUri, string requestMethod = "GET")
        {
            HttpWebRequest req = GetHttpClient(requestUri);
            MemoryStream ret = new MemoryStream(); 
            req.Method = requestMethod;
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            response.GetResponseStream().CopyTo(ret);
            ret.Seek(0, SeekOrigin.Begin);
            return ret; 
        }

        /// <summary>
        /// Parses javascript out of stream content 
        /// </summary>
        /// <param name="contentStream"></param>
        /// <returns></returns>
        private string GetScriptData(Stream contentStream)
        {
            string scriptData = String.Empty;
            using (StreamReader sr = new StreamReader(contentStream))
            {
                scriptData = Utilities.GetScriptData(sr);
                sr.Close();
            }
            return scriptData;
        }
        
        /// <summary>
        /// Gets a generic HttpWebRequest for use in the application 
        /// </summary>
        /// <param name="uri">Request URI</param>
        /// <returns></returns>
        private HttpWebRequest GetHttpClient(string uri)
        {
            HttpWebRequest r = (HttpWebRequest)WebRequest.Create(uri);
            r.Credentials = new NetworkCredential(_routerUsername, _routerPassword);
            r.CookieContainer = _cookieContainer;
            r.PreAuthenticate = true; 
            return r; 
        }

        public string RouterAddress
        {
            get
            {
                return _routerAddress; 
            }
        }
    }
}
