using IESandDACadmt.ViewModel;

namespace IESandDACadmt.Model
{
    public static class ServerDetectionLogic
    {
        public static DbSqlSpControllerData.ServerType CheckServerType(DbSqlSpController theLiveData, IESandDACadmt.ViewModel.ServerDetectionData theServerDetectionData)
        {
            Model.Sql.SqlConnectionStringCheck.CheckForSqlServerString(theServerDetectionData.EmssConnectionStringRegistryLocation,
                                                                 theServerDetectionData.EmssConnectionStringRegistryWowLocation,
                                                                 theServerDetectionData.EmssConnectionStringRegistryItem, 
                                                                 theLiveData);
            if (theLiveData.DbSqlSpControllerData.SqlConnectionStringFound)
            {
                return DbSqlSpControllerData.ServerType.EMSS;
            }
            else
            {
                Model.Sql.SqlConnectionStringCheck.CheckForSqlServerString(theServerDetectionData.EsConnectionStringRegistryLocation,
                                                                     theServerDetectionData.EsConnectionStringRegistryWowLocation,
                                                                     theServerDetectionData.EsConnectionStringRegistryItem,
                                                                     theLiveData);
                if (theLiveData.DbSqlSpControllerData.SqlConnectionStringFound)
                {
                    return DbSqlSpControllerData.ServerType.ES;
                }
            }
            return DbSqlSpControllerData.ServerType.UNKNOWN;
        }
    }
}
