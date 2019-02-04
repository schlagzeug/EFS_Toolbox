using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ToolBox.Utility
{
    public static class NetworkUtility
    {
        public static List<string> GetActiveIPAddresses()
        {
            var list = new List<string>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    list.Add(ip.ToString());
                }
            }

            return list;
        }

        public static List<string> GetPhysicalIPAddresses()
        {
            var list = new List<string>();
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (addr != null && !addr.Address.ToString().Equals("0.0.0.0"))
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                list.Add(ip.Address.ToString());
                            }
                        }
                    }
                }
            }
            return list;
        }

        public static string GetCurrentIPAddress()
        {
            var x = CommonUtility.GetCommonList(GetActiveIPAddresses(), GetPhysicalIPAddresses());
            if (x.Count == 1)
            {
                return x[0];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
