using System;
using System.Collections.Generic;
using System.Linq;
using Noesis.Javascript;

namespace Actiontec.GT784WN
{
    public class ModemStatus
    {

        #region Private Variables 

        // IP 
        private string usrname;
        private string status;
        private string mac_address;
        private string encaps;
        private string gateway;
        private string ip_addr;
        private string[] dnsServers;
        private string dhcpType;

        // DSL 
        private string phys_status;
        private int uprate;
        private int downrate;
        private string rateUnits = "Kbps";

        // Device Info
        private string modelNumber;
        private string serialNumber;
        private string softwareVersion;


        #endregion Private Variables

        #region Properties

        public bool IPConnected { get { return status == "Connected";  } }
        public string Username {get { return usrname; }}
        public string IPStatus { get { return status;  } }
        public string MACAddress { get { return mac_address; } }
        public string Encapsulation { get { return encaps;  } }
        public string GatewayAddress { get { return gateway; } }
        public string IPAddress { get { return ip_addr;  } }
        public string[] DNSServers { get { return dnsServers;  } }
        public string DHCPType { get { return dhcpType;  } }

        public string DSLStatus { get { return phys_status; } }
        public int DSLUpRate { get { return uprate; } }
        public int DSLDownRate { get { return downrate;  } }
        public bool DSLConnected { get { return phys_status == "Up";  } }

        public string DeviceModelNumber { get { return modelNumber; } }
        public string DeviceSerialNumber { get { return serialNumber; } }
        public string DeviceSoftwareVersion { get { return softwareVersion;  } }
        
        #endregion Properties 

        public ModemStatus(string responseScriptData)
        {
            parseScriptData(responseScriptData);
        }

        /* This method was reverse engineered from the outut of the URI "/modemstatus_home.html" on my Actiontec home router */
        private void parseScriptData(string scriptData)
        {
            if (!String.IsNullOrEmpty(scriptData))
            {
                JavascriptContext jsContext = new JavascriptContext();

                jsContext.Run(scriptData);

                string[] wanNameTypes = jsContext.GetParameter("wanInfNames").ToString().Split('|');
                string[] nameType = wanNameTypes[0].Split(':');
                string[] wanEntryInfo = nameType[4].Split(';');
                string dns = String.Empty;
                string mac_addrtmp = String.Empty;
                string dnsIfc = nameType[1];
                string ISP_proto = nameType[3];
                
                // IP Info 
                if (ISP_proto == "PPPoE" || ISP_proto == "PPPoA")
                {
                    usrname = wanEntryInfo[0];
                    status = wanEntryInfo[7];
                    mac_addrtmp = wanEntryInfo[8];

                    if (!String.IsNullOrEmpty(mac_addrtmp))
                    {
                        mac_address = Utilities.FormatMacAddress(mac_addrtmp); 

                    }
                    encaps = wanEntryInfo[9];

                    if (status == "Connected")
                    {
                        ip_addr = wanEntryInfo[10]; 
                    }

                    gateway = wanEntryInfo[11];

                    dnsServers = Utilities.GetDNSArray(wanEntryInfo[12]); 
                    
                }
                else if (ISP_proto == "Bridge")
                {
                    ISP_proto = "Transparent Bridging";
                    status = nameType[7];
                }
                else
                {
                    dhcpType = wanEntryInfo[0];
                    status = wanEntryInfo[7];
                    mac_addrtmp = wanEntryInfo[8];

                    if (!String.IsNullOrEmpty(mac_addrtmp))
                    {
                        mac_address = Utilities.FormatMacAddress(mac_addrtmp);
                    }

                    encaps = wanEntryInfo[9];

                    if (status == "Connected")
                    {
                        ip_addr = wanEntryInfo[10];
                    }

                    dnsServers = Utilities.GetDNSArray(wanEntryInfo[12]);

                    if (dhcpType == "0")
                        ISP_proto = "1483 via DHCP";
                    else
                        ISP_proto = "1483 via Static IP";

                }

                // Device Info
                if (jsContext.GetParameter("devinfo") != null &&
                !String.IsNullOrEmpty(jsContext.GetParameter("devinfo").ToString()))
                {
                    string[] devInfo = jsContext.GetParameter("devinfo").ToString().Split('|');
                    modelNumber = devInfo[0];
                    serialNumber = devInfo[1];
                    softwareVersion = devInfo[2]; 
                }

                // DSL Info 
                if (jsContext.GetParameter("dslstatus") != null &&
                !String.IsNullOrEmpty(jsContext.GetParameter("dslstatus").ToString()))
                {
                    string[] dslStatus = jsContext.GetParameter("dslstatus").ToString().Split('|');
                    phys_status = dslStatus[0];
                    if (phys_status == "Up")
                    {
                        uprate = int.Parse(jsContext.GetParameter("uprate").ToString());
                        downrate = int.Parse(jsContext.GetParameter("downrate").ToString());
                    }
                }
                else
                {
                    phys_status = "Disconnected"; 
                }
            }
            
            
         }
    }
}
