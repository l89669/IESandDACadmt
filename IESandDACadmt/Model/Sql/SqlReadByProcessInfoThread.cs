using System.Data;
using IESandDACadmt.Model;

namespace IESandDACadmt.Model.Sql
{
    class SqlReadByProcessInfoThread
    {
        private string _sqlServerConnectionString = "";
        private int _sqlCommandTimeout = 0;
        private string _sqlQuery = "";
        private DataTable _dataTableResults;
        private DbSqlSpController _liveDbSqlSpController = null;
        

        public SqlReadByProcessInfoThread(DbSqlSpController theLiveDbSqlSpController, string sqlServerConnectionString, int sqlCommandTimeout, string sqlQuery, DataTable theDataTable)
        {
            _liveDbSqlSpController = theLiveDbSqlSpController;
            _sqlServerConnectionString = sqlServerConnectionString;
            _sqlCommandTimeout = sqlCommandTimeout;
            _sqlQuery = sqlQuery;
            _dataTableResults = theDataTable;
        }

        public void ReadByProcessSqlInfo()
        {
            Logging.ActionOutcome readActionOutcome = QuerySqlServer.RunSqlQueryIntoDataTable(_sqlServerConnectionString, _sqlCommandTimeout, _sqlQuery, _dataTableResults);
            if (readActionOutcome.Success)
            {
                _liveDbSqlSpController.ByProcessQuerySuccess = true;
            }
            else
            {
                _liveDbSqlSpController.ByProcessQuerySuccess = false;
                _liveDbSqlSpController.ByProcessQueryMessage = readActionOutcome.Message;
            }
        }
    }
}
