using System; 
using System.Net;
using System.IO;
using HtmlKit;
using System.Text;


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

        /// <summary>
        /// Used to request the data exposed on the "Modem Utilization" admin page 
        /// </summary>
        /// <returns>A ModemUtilization data structure containing requested data</returns>
        public ModemUtilization GetModemUtilization()
        {
           return new ModemUtilization(RequestScriptData( String.Format(_uriTemplate, _routerAddress, Constants.ModemUtilizationUri))); 
        }

        /// <summary>
        /// Used to request the data exposed on the "Modem Status" page 
        /// </summary>
        /// <returns>A ModemStatus data structure containing requested data</returns>
        public ModemStatus GetStatus()
        {
            return new ModemStatus(RequestScriptData(String.Format(_uriTemplate, _routerAddress, Constants.ModemStatusUri))); 
        }

        /// <summary>
        /// Constructs a request to get the javascript portion of the response body from a URI
        /// </summary>
        /// <param name="requestUri">Request URI</param>
        /// <param name="method">HTTP method to use, defaults to "GET"</param>
        /// <returns>String containing content of all <SCRIPT> tags in HTTP response body</returns>
        private string RequestScriptData(string requestUri, string method="GET")
        {
            HttpWebRequest req = GetHttpClient(requestUri);
            req.Method = method; 
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            string scriptData = String.Empty;

            
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
