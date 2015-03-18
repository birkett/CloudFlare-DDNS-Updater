using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace CloudFlare_DDNS
{
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
        }


        /// <summary>
        /// Write an entry to the log
        /// </summary>
        /// <param name="szMessage"></param>
        /// <param name="logLevel"></param>
        public static void log(string szMessage, Level logLevel = 0)
        {
            writeFormControl(szMessage, logLevel);
            writeEventLog(szMessage, logLevel);
        }


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
        }


    }
}
