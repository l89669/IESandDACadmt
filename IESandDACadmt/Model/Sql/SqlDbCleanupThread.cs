using System;
using System.Data;
using System.Data.SqlClient;
using IESandDACadmt.Model.Logging;

// ***************************************


namespace IESandDACadmt.Model.Sql
{
    class SqlDbCleanupThread
    {
        private DbSqlSpController _dbSqlSpController;
        private int _returnedProcessedRows = 0;
        private Model.Logging.ILogging _theLogger;

        public event BatchProcessedEventHandler BatchProcessed;

        public delegate void BatchProcessedEventHandler(object sender, SqlDeletionEventargs e);

        public SqlDbCleanupThread(DbSqlSpController theDbSqlSpController, Model.Logging.ILogging theLogger)
        {
            _dbSqlSpController = theDbSqlSpController;
            _theLogger = theLogger;
        }

        public void StartProcessing()
        {
            try
            {
                while ((_dbSqlSpController.DbSqlSpControllerData.StopController == false) && (DateTime.Now < _dbSqlSpController.DbSqlSpControllerData.ProcessingEndTime)
                        && _dbSqlSpController.DbSqlSpControllerData.RemainingRowsToPurge > 0)
                {
                    using (var conn = new SqlConnection(_dbSqlSpController.DbSqlSpControllerData.SqlConnectionString))
                    using (var command = new SqlCommand(_dbSqlSpController.DbSqlSpControllerData.RecordDeletionStoredProcedureName, conn) { CommandType = CommandType.StoredProcedure })
                    {
                        command.CommandTimeout = 0;
                        SqlParameter spParam2 = new SqlParameter("@batchSize", SqlDbType.Int, 11)
                        {
                            Value = _dbSqlSpController.DbSqlSpControllerData.RecordsForBatchSize
                        };
                        command.Parameters.Add(spParam2);

                        SqlParameter spReturnParam1 = new SqlParameter("@RecordCount", SqlDbType.Int, 11)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(spReturnParam1);

                        if (_dbSqlSpController.DbSqlSpControllerData.CutOffDays)
                        {
                            SqlParameter spParam1 = new SqlParameter("@cutOffDate", SqlDbType.DateTime, 11)
                            {
                                Value = _dbSqlSpController.DbSqlSpControllerData.CutOffDate
                            };
                            command.Parameters.Add(spParam1);
                        }

                        if (_dbSqlSpController.DbSqlSpControllerData.SelectedComputer != "all")
                        {
                            SqlParameter spParam3 = new SqlParameter("@WorkstationId", SqlDbType.NVarChar, 255)
                            {
                                Value = _dbSqlSpController.DbSqlSpControllerData.EpsGuid.ToString()
                            };
                            command.Parameters.Add(spParam3);
                            SqlParameter spParam5 = new SqlParameter("@theWorkstationName", SqlDbType.NVarChar, 255)
                            {
                                Value = _dbSqlSpController.DbSqlSpControllerData.SelectedComputer.ToString()
                            };
                            command.Parameters.Add(spParam5);
                        }

                        if (_dbSqlSpController.DbSqlSpControllerData.SelectedUser != "everyone")
                        {
                            SqlParameter spParam4 = new SqlParameter("@theUserAccountSid", SqlDbType.NVarChar, 200)
                            {
                                Value = _dbSqlSpController.DbSqlSpControllerData.UserSid.ToString()
                            };
                            command.Parameters.Add(spParam4);
                        }

                        if (_dbSqlSpController.DbSqlSpControllerData.SelectedProcess != "all")
                        {
                            SqlParameter spParam6 = new SqlParameter("@theProcessName", SqlDbType.NVarChar, 255)
                            {
                                Value = _dbSqlSpController.DbSqlSpControllerData.SelectedProcess.ToString()
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
                _dbSqlSpController.DbSqlSpControllerData.WorkerCompleted = true;
            }
            catch (Exception ex)
            {
                _theLogger.SaveErrorToLogFile(" Error in thread for Processing Records:" + ex.Message.ToString());
            }
            finally
            {
                _dbSqlSpController.DbSqlSpControllerData.WorkerCompleted = true;
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
