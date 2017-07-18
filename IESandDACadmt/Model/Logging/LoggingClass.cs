using System;
using System.IO;

namespace Lumension_Advanced_DB_Maintenance.Logging
{
    public static class LoggingClass
    {
        
        public static void SaveEventToLogFile(string logFileLocation, string theMessage)
        {
            if (String.IsNullOrEmpty(theMessage)) return;
            theMessage = System.Environment.NewLine + DateTime.Now.ToString() + ":" + theMessage;
            WritetoFile(logFileLocation, theMessage);
        }

        
        public static void SaveErrorToLogFile(string logFileLocation, string theMessage)
        {
            theMessage = System.Environment.NewLine + "*************************************" + System.Environment.NewLine 
                         + DateTime.Now.ToString() + ":" + theMessage + "*************************************" + System.Environment.NewLine;
            WritetoFile(logFileLocation, theMessage);
        }

        private static void WritetoFile(string logFileLocation, string theMessage)
        {
            using (StreamWriter outfile = new StreamWriter(logFileLocation, true))
            {
                outfile.Write(theMessage.ToString());
            }
        }

    }
}
