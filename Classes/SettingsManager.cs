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
namespace CloudFlareDDNS
{
    /// <summary>
    /// Configuration file access wrapper
    /// </summary>
    class SettingsManager
    {


        /// <summary>
        /// Storage for settings, allows the return value to be converted to whatever type needed
        /// </summary>
        public struct Setting
        {
            /// <summary>
            /// Constructor, new setting value from string
            /// </summary>
            /// <param name="szValue"></param>
            public Setting(string szValue)  { m_szValue = szValue; }

            /// <summary>
            /// Constructor, new setting value from boolean
            /// </summary>
            /// <param name="bValue"></param>
            public Setting(bool bValue)     { m_szValue = bValue.ToString(); }

            /// <summary>
            /// Constructor, new setting value from integer
            /// </summary>
            /// <param name="iValue"></param>
            public Setting(int iValue)      { m_szValue = iValue.ToString(Program.cultureInfo); }

            /// <summary>
            /// Accessor, return string
            /// </summary>
            /// <returns></returns>
            public override string  ToString()  { return m_szValue; }

            /// <summary>
            /// Accessor, return boolean
            /// </summary>
            /// <returns></returns>
            public bool             ToBool()    { return System.Convert.ToBoolean(m_szValue, Program.cultureInfo); }

            /// <summary>
            /// Accessor, return integer
            /// </summary>
            /// <returns></returns>
            public int              ToInt()     { return System.Convert.ToInt32(m_szValue, Program.cultureInfo); }

            /// <summary>
            /// Storage
            /// </summary>
            private string m_szValue;
        }//end Setting


        /// <summary>
        /// Get a setting from the config file by a given name
        /// </summary>
        /// <param name="szName"></param>
        /// <returns>String value</returns>
        public static Setting getSetting(string szName)
        {
            return new Setting(Properties.Settings.Default[szName].ToString());

        }//end getSetting() String


        /// <summary>
        /// Set a setting in the user config file
        /// </summary>
        /// <param name="szName"></param>
        /// <param name="szValue"></param>
        public static void setSetting(string szName, string szValue)
        {
            Properties.Settings.Default[szName] = szValue;

        }//end setSetting()


        /// <summary>
        /// Save the user config
        /// </summary>
        public static void saveSettings()
        {
            Properties.Settings.Default.Save();

        }//end saveSettings()


        /// <summary>
        /// Reload the config file from disk
        /// Useful for when the GUI changes the config and the service needs to reload
        /// </summary>
        public static void reloadSettings()
        {
            Properties.Settings.Default.Reload();

        }//end reloadSettings()


    }//end class
}//end namespace
