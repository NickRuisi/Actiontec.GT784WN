using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noesis.Javascript;
using HtmlKit;
using System.IO; 

namespace Actiontec.GT784WN
{
    public class Utilities
    {
        public static string GetScriptData(StreamReader streamReader)
        {
            StringBuilder sb = new StringBuilder();
            var tokenizer = new HtmlTokenizer(streamReader);
            HtmlToken token;
            
            while (tokenizer.ReadNextToken(out token))
            {
                switch (token.Kind)
                {
                    case HtmlTokenKind.ScriptData:
                        var data = (HtmlScriptDataToken)token;
                        sb.Append(data.Data);
                        break;
                    default:
                        break;
                }
            }
            return sb.ToString();
        }

        public static string FormatMacAddress(string macAddressIn)
        {
            string[] mac_array = macAddressIn.Split('-');
            return mac_array[0] + ":" + mac_array[1] + ":" + mac_array[2] + ":" + mac_array[3] + ":" + mac_array[4] + ":" + mac_array[5];
        }

        public static string[] GetDNSArray(string dns)
        {
            if (!String.IsNullOrEmpty(dns))
            {
                if (dns.IndexOf(",") > -1)
                {
                    return  dns.Split(',');
                }
                else
                {
                    return new string[] { dns, null };
                }
            }
            return null; 
        }
    }
}
