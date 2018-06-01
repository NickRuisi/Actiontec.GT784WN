using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiontec.GT784WN
{
    /// <summary>
    /// Encapsulates the Device/Session Info Grid data exposed at the /modemstatus_modemutilization.html URI
    /// </summary>
    public class DeviceSessionInfo
    {
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public int SessionCount { get; set; }
    }
}
