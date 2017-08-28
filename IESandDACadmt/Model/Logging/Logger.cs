using System;
using System.IO;

namespace IESandDACadmt.Model.Logging
{
    public class Logger : ILogging
    {
        private string _logFileLocation;

        public string LogFileLocation
        {
            get { return _logFileLocation; }
            set { _logFileLocation = value; }
        }

        private object theLock = new object();

        public Logger()
        {
            _logFileLocation = Environment.CurrentDirectory.ToString() + "\\" + System.AppDomain.CurrentDomain.FriendlyName + ".log";
        }

        public Logger(string logFileLocation)
        {
            _logFileLocation = logFileLocation;
        }


        public void SaveEventToLogFile(string theMessage)
        {
            if (String.IsNullOrEmpty(theMessage)) return;
            theMessage = System.Environment.NewLine + DateTime.Now.ToString() + ":" + theMessage;
            WritetoFile(theMessage);
        }

        
        public void SaveErrorToLogFile(string theMessage)
        {
            theMessage = System.Environment.NewLine + "*************************************" + System.Environment.NewLine 
                         + DateTime.Now.ToString() + ":" + theMessage + "*************************************" + System.Environment.NewLine;
            WritetoFile(theMessage);
        }

        private void WritetoFile(string theMessage)
        {
            lock (theLock)
            {
                using (StreamWriter outfile = new StreamWriter(_logFileLocation, true))
                {
                    outfile.Write(theMessage.ToString());
                }
            }
        }

    }
}
