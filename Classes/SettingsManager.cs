/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2014-2015 Anthony Birkett
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Configuration file access wrapper
    /// </summary>
    internal class SettingsManager
    {
        /// <summary>
        /// Sorage for unserialized settings
        /// </summary>
        private List<Setting> m_Settings;

        /// <summary>
        /// Store the config location
        /// </summary>
        private string m_szConfigFilePath;

        /// <summary>
        /// Class constructor
        /// </summary>
        public SettingsManager()
        {
            m_Settings = new List<Setting>();
            m_szConfigFilePath = null;
            loadSettings();
        }//end SettingsManager()

        /// <summary>
        /// Storage for settings, allows the return value to be converted to whatever type needed
        /// </summary>
        public struct Setting
        {
            /// <summary>
            /// Constructor, new setting value from string
            /// </summary>
            /// <param name="szName"></param>
            /// <param name="szValue"></param>
            public Setting(string szName, string szValue) { m_szName = szName; m_szValue = szValue; }

            /// <summary>
            /// Constructor, new setting value from boolean
            /// </summary>
            /// <param name="szName"></param>
            /// <param name="bValue"></param>
            public Setting(string szName, bool bValue) { m_szName = szName; m_szValue = bValue.ToString(); }

            /// <summary>
            /// Constructor, new setting value from integer
            /// </summary>
            /// <param name="szName"></param>
            /// <param name="iValue"></param>
            public Setting(string szName, int iValue) { m_szName = szName; m_szValue = iValue.ToString(Program.cultureInfo); }

            /// <summary>
            /// Accessor, return string
            /// </summary>
            /// <returns></returns>
            public override string ToString() { return m_szValue; }

            /// <summary>
            /// Accessor, return boolean
            /// </summary>
            /// <returns></returns>
            public bool ToBool() { return System.Convert.ToBoolean(m_szValue, Program.cultureInfo); }

            /// <summary>
            /// Accessor, return integer
            /// </summary>
            /// <returns></returns>
            public int ToInt() { return System.Convert.ToInt32(m_szValue, Program.cultureInfo); }

            /// <summary>
            /// Store the value
            /// </summary>
            public string m_szValue;

            /// <summary>
            /// Store the name
            /// </summary>
            public string m_szName;
        }//end Setting

        /// <summary>
        /// Get a setting from the config file by a given name
        /// </summary>
        /// <param name="szName"></param>
        /// <returns>String value</returns>
        public Setting getSetting(string szName)
        {
            return m_Settings.Find(x => x.m_szName.Equals(szName));
        }//end getSetting() String

        /// <summary>
        /// Set a setting in the user config file
        /// </summary>
        /// <param name="szName"></param>
        /// <param name="szValue"></param>
        public void setSetting(string szName, string szValue)
        {
            //Find and remove the current setting value
            Setting found = m_Settings.Find(x => x.m_szName.Equals(szName));
            m_Settings.Remove(found);
            //Add the new setting value
            m_Settings.Add(new Setting(szName, szValue));
        }//end setSetting()

        /// <summary>
        /// Create the folder in %appdata%, and the config file
        /// </summary>
        private void createSettingsFile()
        {
            string appdataRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Console.WriteLine("Pfad:"+ Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString());
            string configFolder = Path.Combine(appdataRoaming, "CloudFlareDDNS");

            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            m_szConfigFilePath = Path.Combine(configFolder, "user.config");

            if (!File.Exists(m_szConfigFilePath))
            {
                FileStream file = File.Create(m_szConfigFilePath);
                file.Close();
            }
        }//end createSettingsFile()

        /// <summary>
        /// Save application settings to file
        /// </summary>
        public void saveSettings()
        {
            createSettingsFile();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            StreamWriter fileStream = new StreamWriter(m_szConfigFilePath);
            string json = ser.Serialize(m_Settings);
            fileStream.Write(json);
            fileStream.Close();
        }//end saveSettings()

        /// <summary>
        /// Load application settings from file
        /// </summary>
        public void loadSettings()
        {
            createSettingsFile();
            setDefaults();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            StreamReader fileStream = new StreamReader(m_szConfigFilePath);
            string json = fileStream.ReadToEnd();
            fileStream.Close();
            //Dont null the list if the file is empty
            if (!String.IsNullOrEmpty(json))
            {
                m_Settings = ser.Deserialize<List<Setting>>(json);
            }
        }//end loadSettings()

        /// <summary>
        /// Load in some default settings for first launch
        /// </summary>
        private void setDefaults()
        {
            m_Settings.Add(new Setting("FetchTime", 10));
            m_Settings.Add(new Setting("Domain", ""));
            m_Settings.Add(new Setting("EmailAddress", ""));
            m_Settings.Add(new Setting("APIKey", ""));
            m_Settings.Add(new Setting("SelectedHosts", ""));
            m_Settings.Add(new Setting("ExternalAddressIPv4", ""));
            m_Settings.Add(new Setting("ExternalAddressIPv6", ""));
            m_Settings.Add(new Setting("UseEventLog", true));
            m_Settings.Add(new Setting("IPV4UpdateURL", "https://api.ipify.org"));
            m_Settings.Add(new Setting("IPV6UpdateURL", "https://api64.ipify.org/"));
            m_Settings.Add(new Setting("UseLocalInfos", false));
            m_Settings.Add(new Setting("APIUrl", "https://api.cloudflare.com/client/v4"));
            m_Settings.Add(new Setting("SelectedZone", ""));
            m_Settings.Add(new Setting("UseInternalIP", false));
            m_Settings.Add(new Setting("HideSRV", false));
            m_Settings.Add(new Setting("StartMinimized", false));
            m_Settings.Add(new Setting("DefaultInterface", ""));
        }//end setDefaults()
    }//end class
}//end namespace