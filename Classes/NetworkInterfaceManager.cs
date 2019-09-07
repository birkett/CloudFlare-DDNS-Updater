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
        public static string GetDefaultIP()
        {
            string currentLoadedValue = Program.settingsManager.getSetting("DefaultInterface").ToString();
            if (loadedValue == null && loadedValue != currentLoadedValue)
            {
                NetworkInterface netif = GetCurrentDefaultInterface();
                IPInterfaceProperties properties = netif.GetIPProperties();
                foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.Address.ToString();
                    }
                }
                result = properties.UnicastAddresses[0].Address.ToString();
            }
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
