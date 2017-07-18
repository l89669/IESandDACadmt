namespace IESandDACadmt.Model
{
    public static class ServerDetectionLogic
    {
        public static DbSqlSpController.ServerType CheckServerType(DbSqlSpController theLiveData, IESandDACadmt.ViewModel.ServerDetectionData theServerDetectionData)
        {
            Model.Sql.SqlConnectionStringCheck.CheckForSqlServerString(theServerDetectionData.EmssConnectionStringRegistryLocation,
                                                                 theServerDetectionData.EmssConnectionStringRegistryWowLocation,
                                                                 theServerDetectionData.EmssConnectionStringRegistryItem, 
                                                                 theLiveData);
            if (theLiveData.SqlConnectionStringFound)
            {
                return DbSqlSpController.ServerType.EMSS;
            }
            else
            {
                Model.Sql.SqlConnectionStringCheck.CheckForSqlServerString(theServerDetectionData.EsConnectionStringRegistryLocation,
                                                                     theServerDetectionData.EsConnectionStringRegistryWowLocation,
                                                                     theServerDetectionData.EsConnectionStringRegistryItem,
                                                                     theLiveData);
                if (theLiveData.SqlConnectionStringFound)
                {
                    return DbSqlSpController.ServerType.ES;
                }
            }
            return DbSqlSpController.ServerType.UNKNOWN;
        }
    }
}
