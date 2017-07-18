using System;
using System.Data;
using System.Data.SqlClient;
using Lumension_Advanced_DB_Maintenance.Data;
using Lumension_Advanced_DB_Maintenance.Logging;

// ***************************************


namespace Lumension_Advanced_DB_Maintenance.Sql
{
    class SqlDbCleanupThread
    {
        private DbSqlSpController _dbSqlSpController;
        private int _returnedProcessedRows = 0;

        public event BatchProcessedEventHandler BatchProcessed;

        public delegate void BatchProcessedEventHandler(object sender, SqlDeletionEventargs e);

        public SqlDbCleanupThread(DbSqlSpController theDbSqlSpController)
        {
            _dbSqlSpController = theDbSqlSpController;
        }

        public void StartProcessing()
        {
            try
            {
                while ((_dbSqlSpController.StopController == false) && (DateTime.Now < _dbSqlSpController.ProcessingEndTime)
                        && _dbSqlSpController.RemainingRowsToPurge > 0)
                {
                    using (var conn = new SqlConnection(_dbSqlSpController.SqlConnectionString))
                    using (var command = new SqlCommand(_dbSqlSpController.RecordDeletionStoredProcedureName, conn) { CommandType = CommandType.StoredProcedure })
                    {
                        command.CommandTimeout = 0;
                        SqlParameter spParam2 = new SqlParameter("@batchSize", SqlDbType.Int, 11)
                        {
                            Value = _dbSqlSpController.RecordsForBatchSize
                        };
                        command.Parameters.Add(spParam2);

                        SqlParameter spReturnParam1 = new SqlParameter("@RecordCount", SqlDbType.Int, 11)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(spReturnParam1);

                        if (_dbSqlSpController.CutOffDays)
                        {
                            SqlParameter spParam1 = new SqlParameter("@cutOffDate", SqlDbType.DateTime, 11)
                            {
                                Value = _dbSqlSpController.CutOffDate
                            };
                            command.Parameters.Add(spParam1);
                        }

                        if (_dbSqlSpController.SelectedComputer != "all")
                        {
                            SqlParameter spParam3 = new SqlParameter("@WorkstationId", SqlDbType.NVarChar, 255)
                            {
                                Value = _dbSqlSpController.EpsGuid.ToString()
                            };
                            command.Parameters.Add(spParam3);
                            SqlParameter spParam5 = new SqlParameter("@theWorkstationName", SqlDbType.NVarChar, 255)
                            {
                                Value = _dbSqlSpController.SelectedComputer.ToString()
                            };
                            command.Parameters.Add(spParam5);
                        }

                        if (_dbSqlSpController.SelectedUser != "everyone")
                        {
                            SqlParameter spParam4 = new SqlParameter("@theUserAccountSid", SqlDbType.NVarChar, 200)
                            {
                                Value = _dbSqlSpController.UserSid.ToString()
                            };
                            command.Parameters.Add(spParam4);
                        }

                        if (_dbSqlSpController.SelectedProcess != "all")
                        {
                            SqlParameter spParam6 = new SqlParameter("@theProcessName", SqlDbType.NVarChar, 255)
                            {
                                Value = _dbSqlSpController.SelectedProcess.ToString()
                            };
                            command.Parameters.Add(spParam6);
                        }
                        conn.Open();
                        command.ExecuteNonQuery();
                        _returnedProcessedRows = Convert.ToInt32(spReturnParam1.Value);
                        conn.Close();
                    }
                    // ****************************************************************************
                    SqlDeletionEventargs results = new SqlDeletionEventargs { RecordsDeletedThisBatch = _returnedProcessedRows };
                    OnBatchProcessed(results);
                    // ****************************************************************************
                    
                }
                _dbSqlSpController.WorkerCompleted = true;
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(_dbSqlSpController.LogFileLocation," Error in thread for Processing Records:" + ex.Message.ToString());
            }
            finally
            {
                _dbSqlSpController.WorkerCompleted = true;
            }
        }


        protected virtual void OnBatchProcessed(SqlDeletionEventargs e)
        {
            BatchProcessedEventHandler handler = BatchProcessed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }
}
