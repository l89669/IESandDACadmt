namespace Lumension_Advanced_DB_Maintenance.BL
{
    public static class ServerDetectionLogic
    {
        public static Data.DbSqlSpController.ServerType CheckServerType(Data.DbSqlSpController theLiveData, Data.ServerDetectionData theServerDetectionData)
        {
            Sql.SqlConnectionStringCheck.CheckForSqlServerString(theServerDetectionData.EmssConnectionStringRegistryLocation,
                                                                 theServerDetectionData.EmssConnectionStringRegistryWowLocation,
                                                                 theServerDetectionData.EmssConnectionStringRegistryItem, 
                                                                 theLiveData);
            if (theLiveData.SqlConnectionStringFound)
            {
                return Data.DbSqlSpController.ServerType.EMSS;
            }
            else
            {
                Sql.SqlConnectionStringCheck.CheckForSqlServerString(theServerDetectionData.EsConnectionStringRegistryLocation,
                                                                     theServerDetectionData.EsConnectionStringRegistryWowLocation,
                                                                     theServerDetectionData.EsConnectionStringRegistryItem,
                                                                     theLiveData);
                if (theLiveData.SqlConnectionStringFound)
                {
                    return Data.DbSqlSpController.ServerType.ES;
                }
            }
            return Data.DbSqlSpController.ServerType.UNKNOWN;
        }
    }
}
