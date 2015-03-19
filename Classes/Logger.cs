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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace CloudFlareDDNS
{
    /// <summary>
    /// Provides common functions for logging of events
    /// </summary>
    class Logger
    {


        /// <summary>
        /// Stores the items for a list view control
        /// </summary>
        static public List<ListViewItem> m_LogItems = new List<ListViewItem>();

        /// <summary>
        /// Enum of Log message levels
        /// </summary>
        public enum Level
        {
            Info = 0,
            Warning,
            Error,

        }//end enum


        /// <summary>
        /// Write an entry to the log
        /// </summary>
        /// <param name="szMessage"></param>
        /// <param name="logLevel"></param>
        public static void log(string szMessage, Level logLevel = 0)
        {
            writeFormControl(szMessage, logLevel);

            if(SettingsManager.getSetting("UseEventLog") == "True")
                writeEventLog(szMessage, logLevel);

        }//end log


        /// <summary>
        /// Add messages to the log view
        /// </summary>
        /// <param name="szMessage"></param>
        /// <param name="logLevel"></param>
        private static void writeFormControl(string szMessage, Level logLevel = 0)
        {
            ListViewItem row = new ListViewItem();
            switch (logLevel)
            {
                case Level.Warning:
                    row.ImageIndex = 1;
                    break;

                case Level.Error:
                    row.ImageIndex = 2;
                    break;

                default: //Level.Info
                    row.ImageIndex = 0;
                    break;
            }
            row.SubItems.Add(szMessage);

            m_LogItems.Add(row);

        }//end writeFormControl()


        /// <summary>
        /// Write a message to the Windows Event Log
        /// </summary>
        /// <param name="szMessage"></param>
        /// <param name="logLevel"></param>
        private static void writeEventLog(string szMessage, Level logLevel = 0)
        {
            string sSource = "CloudFlare DDNS Updater";
            string sLog = "Application";

            try
            {
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                switch (logLevel)
                {
                    case Level.Warning:
                        EventLog.WriteEntry(sSource, szMessage, EventLogEntryType.Warning);
                        break;

                    case Level.Error:
                        EventLog.WriteEntry(sSource, szMessage, EventLogEntryType.Error);
                        break;

                    default: //Level.Info
                        EventLog.WriteEntry(sSource, szMessage, EventLogEntryType.Information);
                        break;
                }
            }
            catch {}

        }//end writeEventLog()


    }//end class
}//end namespace
