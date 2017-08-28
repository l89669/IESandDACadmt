namespace IESandDACadmt.Model.Logging
{
    public interface ILogging
    {
        string LogFileLocation { get; set; }

        void SaveErrorToLogFile(string theMessage);
        void SaveEventToLogFile(string theMessage);
    }
}