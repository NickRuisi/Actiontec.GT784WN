using System;
using Noesis.Javascript;
using System.Collections.Generic;

namespace Actiontec.GT784WN
{
    public class ModemUtilization
    {
        #region Private Variables

        private int total_mem;
        private int mem_usage;
        private int max_session_num;
        private int tcp_session;
        private int udp_session;
        private int total_sessions;

        private List<DeviceSessionInfo> deviceSessionLog = new List<DeviceSessionInfo>();

        #endregion Private Variables

        #region Properties

        public int TotalMemoryMB { get { return total_mem; } }
        public int MemoryUsageMB { get { return mem_usage; } }
        public int MaxSessionCount { get { return max_session_num; } }
        public int TcpSessionCount { get { return tcp_session; } }
        public int UdpSessionCount { get { return udp_session; } }
        public int TotalSessionCount { get { return total_sessions; } }

        #endregion Properties

        public ModemUtilization(string responseScriptData)
        {
            parseScriptData(responseScriptData);
        }

        /* Method reverse engineered/adapted from the script emitted at /modemstatus_modemutilization.html from the device  */
        private void parseScriptData(string scriptData)
        {
            // We don't want to call this because it will cause a reference error 
            scriptData = scriptData.Replace("print_utilize_log();", String.Empty); 

            if (!String.IsNullOrEmpty(scriptData))
            {
                JavascriptContext jsContext = new JavascriptContext();
                
                jsContext.Run(scriptData);

                total_mem = int.Parse(jsContext.GetParameter("total_mem").ToString());
                mem_usage = int.Parse(jsContext.GetParameter("total_mem").ToString());
                max_session_num = int.Parse(jsContext.GetParameter("max_session_num").ToString());
                tcp_session = int.Parse(jsContext.GetParameter("tcp_session").ToString());
                udp_session = int.Parse(jsContext.GetParameter("udp_session").ToString());
                total_sessions = int.Parse(jsContext.GetParameter("total_sessions").ToString());


                if (jsContext.GetParameter("modem_utlize_log") != null && !String.IsNullOrEmpty(jsContext.GetParameter("modem_utlize_log").ToString()))
                {
                    string[] client_info = jsContext.GetParameter("modem_utlize_log").ToString().Split('|');

                    foreach (string s in client_info)
                    {
                        string[] clientDetail = s.Split('/');

                        deviceSessionLog.Add(new DeviceSessionInfo() { DeviceName = clientDetail[0], IPAddress = clientDetail[1], SessionCount = int.Parse(clientDetail[2]) });
                    }
                }


            }
        }
        
    }
}
