using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CloudFlare_DDNS
{
    class Logger
    {
        /// <summary>
        /// Used to store a reference to the ListView control on a form to populate with log entries
        /// </summary>
        static private System.Windows.Forms.ListView m_ListView = null;

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
        /// Set up the ListView control to send events to
        /// </summary>
        /// <param name="listView"></param>
        public static void setTargetControl(ref System.Windows.Forms.ListView listView)
        {
            m_ListView = listView;
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
        /// <param name="message"></param>
        /// <param name="level"></param>
        private static void writeFormControl(string szMessage, Level logLevel = 0)
        {
            if(m_ListView == null)
                return;

            System.Windows.Forms.ListViewItem row = m_ListView.Items.Add("");
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
    }
}
