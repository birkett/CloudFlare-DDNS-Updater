using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CloudFlareDDNS
{
    class NetworkInterfaceManager
    {
        private static string loadedValue = null;
        private static string result = "";
        public static string GetDefaultIP(System.Net.Sockets.AddressFamily addressFamily = System.Net.Sockets.AddressFamily.InterNetwork)
        {
            string currentLoadedValue = Program.settingsManager.getSetting("DefaultInterface").ToString();

                NetworkInterface netif = GetCurrentDefaultInterface();
                IPInterfaceProperties properties = netif.GetIPProperties();
                foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == addressFamily)
                    {
                        return ip.Address.ToString();
                    }
                }
                result = properties.UnicastAddresses[0].Address.ToString();
            
            return result;
        }

        /**
         * Function forwarding ...
         */
        public static List<NetworkInterface> GetAllNetworkInterfaces()
        {
            List<NetworkInterface> list = NetworkInterface.GetAllNetworkInterfaces().ToList();
            return list;
        }

        public static NetworkInterface GetCurrentDefaultInterface()
        {
            NetworkInterface networkInterface = null;

            string selectedInterface = Program.settingsManager.getSetting("DefaultInterface").ToString();
            if (selectedInterface != "") {
                foreach (NetworkInterface netif in GetAllNetworkInterfaces())
                {

                    if (netif.Name == selectedInterface)
                    {
                        networkInterface = netif;
                        break;
                    }

                }
            }
            if(networkInterface == null)
            {
                return GetDeviceDefaultInterface();
            }
            return networkInterface;
        }

        public static NetworkInterface GetDeviceDefaultInterface()
        {
            var nic = NetworkInterface
      .GetAllNetworkInterfaces()
      .FirstOrDefault(i => i.NetworkInterfaceType != NetworkInterfaceType.Loopback && i.NetworkInterfaceType != NetworkInterfaceType.Tunnel);
            return nic;
          }
    }
}
