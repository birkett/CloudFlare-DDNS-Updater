using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFlare_DDNS
{
    class SettingsManager
    {
        /// <summary>
        /// Get a setting from the config file by a given name
        /// </summary>
        /// <param name="szName"></param>
        /// <returns></returns>
        public static string getSetting(string szName)
        {
            return Properties.Settings.Default[szName].ToString();
        }

        /// <summary>
        /// Set a setting in the user config file
        /// </summary>
        /// <param name="szName"></param>
        /// <param name="szValue"></param>
        public static void setSetting(string szName, string szValue)
        {
            Properties.Settings.Default[szName] = szValue;
        }

        /// <summary>
        /// Save the user config
        /// </summary>
        public static void saveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}
