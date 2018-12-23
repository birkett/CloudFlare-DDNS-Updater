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
        private static List<Entry> m_LogItems = new List<Entry>();

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
        /// LogItem is a generic way of storing messages
        /// </summary>
        public struct Entry
        {
            /// <summary>
            /// Constructor, create a log entry
            /// </summary>
            /// <param name="szMsg"></param>
            /// <param name="eLevel"></param>
            public Entry(string szMsg, Level eLevel) { m_szMsg = szMsg; m_eLevel = eLevel; }


            /// <summary>
            /// Return the entry level
            /// </summary>
            public Level Level { get { return m_eLevel; } }


            /// <summary>
            /// Return the entry message
            /// </summary>
            public string Message { get { return m_szMsg; } }


            /// <summary>
            /// Entry level
            /// </summary>
            private Level  m_eLevel;


            /// <summary>
            /// Entry message
            /// </summary>
            private string m_szMsg;
        }//end LogItem;


        /// <summary>
        /// Write an entry to the log
        /// </summary>
        /// <param name="szMessage"></param>
        /// <param name="logLevel"></param>
        public static void log(string szMessage, Level logLevel = 0)
        {
            m_LogItems.Add(new Entry(szMessage, logLevel));

            if (Program.settingsManager.getSetting("UseEventLog").ToBool())
                writeEventLog(szMessage, logLevel);

        }//end log


        /// <summary>
        /// Property to get the logged items
        /// </summary>
        public static List<Entry> items
        {
            get { return m_LogItems; }
        }


        /// <summary>
        /// Clear out the current log items
        /// </summary>
        public static void reset()
        {
            m_LogItems.Clear();

        }


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
