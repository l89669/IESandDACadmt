namespace IESandDACadmt.ViewModel
{
    public class ServerDetectionData
    {
        private string _emssConnectionStringRegistryLocation = @"SOFTWARE\PatchLink.com\DistributionService";

        public string EmssConnectionStringRegistryLocation
        {
            get { return _emssConnectionStringRegistryLocation; }
        }

        private string _emssConnectionStringRegistryWowLocation = @"SOFTWARE\Wow6432Node\PatchLink.com\DistributionService";
        public string EmssConnectionStringRegistryWowLocation
        {
            get { return _emssConnectionStringRegistryWowLocation; }
        }

        private string _emssConnectionStringRegistryItem = "UPCCommonConnectionString";
        public string EmssConnectionStringRegistryItem
        {
            get { return _emssConnectionStringRegistryItem; }
        }

        private  string _esConnectionStringRegistryLocation = @"SYSTEM\CurrentControlSet\Services\sxs\parameters";
        public string EsConnectionStringRegistryLocation
        {
            get { return _esConnectionStringRegistryLocation; }
        }

        private string _esConnectionStringRegistryWowLocation = @"SYSTEM\CurrentControlSet\Services\sxs\parameters";
        public string EsConnectionStringRegistryWowLocation
        {
            get { return _esConnectionStringRegistryWowLocation; }
        }

        private string _esConnectionStringRegistryItem = "DbConnectionString";
        public string EsConnectionStringRegistryItem
        {
            get { return _esConnectionStringRegistryItem; }
        }

        private string _emssUserReadQuery = "SELECT NTUserName, UserSID FROM dbo.LogUser ORDER BY NTUserName";

        public string EmssUserReadQuery
        {
            get { return _emssUserReadQuery; }
            set { _emssUserReadQuery = value; }
        }

        private string _emssComputerReadQuery = "SELECT ComputerName, EPSGuid FROM dbo.LogComputer ORDER BY ComputerName";

        public string EmssComputerReadQuery
        {
            get { return _emssComputerReadQuery; }
            set { _emssComputerReadQuery = value; }
        }

        private string _esUserReadQuery = " SELECT NTUserName, UserSID FROM [ActivityLog].[User] ORDER BY NTUserName";

        public string EsUserReadQuery
        {
            get { return _esUserReadQuery; }
            set { _esUserReadQuery = value; }
        }

        private string _esComputerReadQuery = "SELECT ComputerName, ComputerID FROM [ActivityLog].[Computer] ORDER BY ComputerName";

        public string EsComputerReadQuery
        {
            get { return _esComputerReadQuery; }
            set { _esComputerReadQuery = value; }
        }

    }
}
