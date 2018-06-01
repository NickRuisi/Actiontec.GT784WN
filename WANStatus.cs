using System;
using HtmlAgilityPack;
using Noesis.Javascript; 

namespace Actiontec.GT784WN
{
    public class WANStatus
    {
        #region Private Variables

        private int up_power;
        private int down_power;
        private int speed_u;
        private int speed_d;
        private int retrains_time;
        private int retrains_num;
        private int Near_endCRC;
        private int Far_endCRC;
        private int Near_endCRC_cnt;
        private int Far_endCRC_cnt;
        private int Near_endRS;
        private int Far_endRS;
        private int Near_endFEC;
        private int Far_endFEC;
        private int discard_pkt_up;
        private int discard_pkt_down;
        private int snr_margin;
        private int snr_margin_u;
        private int attenuation;
        private int attenuation_u;

        #endregion

        #region Properties 

        public int UpstreamPower { get { return up_power; } }
        public int DownstreamPower { get { return down_power; } }
        public int UpstreamSpeed { get { return speed_u; } }
        public int DownstreamSpeed { get { return speed_d; } }
        public int Retrains { get { return retrains_num; } }
        public int RetrainsTime { get { return retrains_time; } }
        public int NearEndCRC { get { return Near_endCRC; } }
        public int FarEndCRC { get { return Far_endCRC; } }
        public int NearEndFEC { get { return Near_endFEC; } }
        public int FarEndFEC { get { return Far_endFEC; } }
        public int NearEndRS { get { return Near_endRS; } }
        public int FarEndRS { get { return Far_endRS; } }
        public int UpstreamDiscardedPackets { get { return discard_pkt_up; } }
        public int DownstreamDiscardedPackets { get { return discard_pkt_down; } }
        public int UpstreamSNR { get { return snr_margin_u; } }
        public int DownstreamSNR { get { return snr_margin; } }
        public int UpstreamAttenuation { get { return attenuation_u; } }
        public int DownstreamAttenuation { get { return attenuation;  } }

        #endregion

        public WANStatus(HtmlDocument dom, string responseScriptData)
        {
            parseScriptData(dom, responseScriptData);
        }

        /// <summary>
        /// Executes the script passed in against the DOM passed
        /// Informed bythe function show_dsl_status() 
        /// which is emitted by the WAN Status URL of the router 
        /// </summary>
        /// <param name="dom"></param>
        /// <param name="scriptData"></param>
        private void parseScriptData(HtmlDocument dom, string scriptData)
        {
            JavascriptContext jsContext = new JavascriptContext();
            jsContext.SetParameter("document", dom); 
            jsContext.Run(scriptData);

            string dslstatus = jsContext.GetParameter("dslstatus").ToString();
            string[] dslsts_temp = dslstatus.Split('|');
            
            speed_d = int.Parse(dslsts_temp[2]);
            speed_d = int.Parse(dslsts_temp[3]);
            retrains_time = int.Parse(dslsts_temp[7]);
            retrains_num = int.Parse( dslsts_temp[6]);
            Near_endCRC = int.Parse(dslsts_temp[8].Split('/')[0]);
            Far_endCRC = int.Parse(dslsts_temp[9].Split('/')[0]);
            Near_endCRC_cnt = int.Parse(dslsts_temp[10].Split('/')[0]);
            Far_endCRC_cnt = int.Parse(dslsts_temp[11].Split('/')[0]);
            Near_endRS = int.Parse(dslsts_temp[12].Split('/')[0]);
            Far_endRS = int.Parse(dslsts_temp[13].Split('/')[0]);
            Near_endFEC = int.Parse(dslsts_temp[14].Split('/')[0]);
            Far_endFEC = int.Parse(dslsts_temp[15].Split('/')[0]);
            discard_pkt_up = int.Parse(dslsts_temp[16].Split('/')[0]);
            discard_pkt_down = int.Parse(dslsts_temp[16].Split('/')[1]);
            up_power = int.Parse(dslsts_temp[17].Split('/')[0]);
            down_power = int.Parse(dslsts_temp[17].Split('/')[1]);
            snr_margin = int.Parse(dslsts_temp[4].Split('/')[0]);
            snr_margin_u = int.Parse(dslsts_temp[4].Split('/')[1]);
            attenuation = int.Parse(dslsts_temp[5].Split('/')[0]);
            attenuation_u = int.Parse(dslsts_temp[5].Split('/')[1]);
         }
    }
}
