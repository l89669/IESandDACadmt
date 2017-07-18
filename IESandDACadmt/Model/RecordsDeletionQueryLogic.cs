using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using IESandDACadmt.Model.Logging;

namespace IESandDACadmt.Model
{
    public static class RecordsDeletionQueryLogic
    {

        public static string CreateRequiredStoredProcedures(DbSqlSpController theLiveData)
        {
            string result = "failure";
            BuildEventsToExclude(theLiveData);
            result = DeleteExistingMaintenanceStoredProcedures(theLiveData);
            if (result == "success")
            {
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " Existing SQL Stored Procedures deleted.");
                string recordDeletionSpSqlCode = BuildRecordDeletionSqlSpString(theLiveData.DbSqlSpControllerData.RecordDeletionStoredProcedureName, theLiveData);
                result = CreateStoredProcedure(theLiveData.DbSqlSpControllerData.LogFileLocation, recordDeletionSpSqlCode, "Record Deletion", theLiveData.DbSqlSpControllerData.SqlConnectionString);
                if (result == "success")
                {
                    LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " SQL Stored Procedure for Record-Deletion created.");
                    string totalRecordsCalcSpSqlCode = BuildTotalRecordCalcSqlString(theLiveData.DbSqlSpControllerData.TotalRecordsCalcStoredProcedureName, theLiveData);
                    result = CreateStoredProcedure(theLiveData.DbSqlSpControllerData.LogFileLocation, totalRecordsCalcSpSqlCode, "Total Record Calculation", theLiveData.DbSqlSpControllerData.SqlConnectionString);
                    if (result == "success")
                    {
                        LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " SQL Stored Procedure for Total-Record-Calculation created.");
                    }
                    else
                    {
                        LoggingClass.SaveErrorToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " Error creating Total-Record-Calculation Stored Procedure:" + result);
                        result = "failure";
                    }
                }
                else
                {
                    LoggingClass.SaveErrorToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " Error creating Record-Deletion Stored Procedure:" + result);
                    result = "failure";
                }
            }
            else
            {
                LoggingClass.SaveErrorToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " Error deleting existing Stored Procedures:" + result);
                result = "failure";
            }
            return result;
        }

        private static bool BuildEventsToExclude(DbSqlSpController theLiveData)
        {
            List<string> eventsToExcludeList = new List<string>();
            foreach (KeyValuePair<string, bool> kvp in theLiveData.DbSqlSpControllerData.EventTypesToDelete)
            {
                if (kvp.Value == false)
                {
                    eventsToExcludeList.Add(kvp.Key.Substring(0, (kvp.Key.IndexOf(":", StringComparison.Ordinal))));
                }
            }
            if (eventsToExcludeList.Count > 0)
            {
                theLiveData.DbSqlSpControllerData.EventsToExclude = String.Join(",", eventsToExcludeList) + ", 12";
            }
            else
            {
                theLiveData.DbSqlSpControllerData.EventsToExclude = String.Join(",", eventsToExcludeList) + "12";
            }
            LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, "The Events list to exclude from deleting:" + theLiveData.DbSqlSpControllerData.EventsToExclude + ":");
            return true;
        }

        private static string CreateStoredProcedure(string LogFileLocation, string spSqlCode, string spAction, string theSqlConnectionString)
        {
            string result = "failure";
            LoggingClass.SaveEventToLogFile(LogFileLocation, " The required SQL Stored Procedure for " + spAction + " does not yet exist.");
            LoggingClass.SaveEventToLogFile(LogFileLocation, " " + spAction + " SQL Stored Procedure assembled:");
            LoggingClass.SaveEventToLogFile(LogFileLocation, spSqlCode);
            SqlConnection sqlDbConnection = new SqlConnection(theSqlConnectionString);
            try
            {
                sqlDbConnection.Open();
                LoggingClass.SaveEventToLogFile(LogFileLocation, " SQL connection for " + spAction + " Stored Procedure creation OPEN.");
                SqlCommand spCreateCommand = new SqlCommand(spSqlCode, sqlDbConnection);
                spCreateCommand.ExecuteNonQuery();
                LoggingClass.SaveEventToLogFile(LogFileLocation, " SQL command to create " + spAction + " Stored Procedure completed.");
                sqlDbConnection.Close();
                result = "success";
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(LogFileLocation, ex.Message);
                result = " Error creation " + spAction + " Stored Procedure:" + ex.Message;
            }
            return result;
        }

        private static string BuildRecordDeletionSqlSpString(string storedProcedureName, DbSqlSpController theLiveData)
        {
            StringBuilder theSpCode = new StringBuilder();
            theSpCode.Append(@"CREATE PROCEDURE [dbo].[" + storedProcedureName + @"]
									(   @batchSize int");
            if (theLiveData.DbSqlSpControllerData.CutOffDays)
            {
                theSpCode.Append(@"	    ,@cutOffDate datetime");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
            {
                theSpCode.Append(@"	    , @theUserAccountSid nvarchar(200)");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
            {
                theSpCode.Append(@"	    , @WorkstationId nvarchar(255)
										, @theWorkstationName nvarchar(255)");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	    , @theProcessName nvarchar(255)");
            }
            theSpCode.Append(@"	    )
														AS
														BEGIN
															--SET NOCOUNT ON
															DECLARE @numberOfRecordsProcessed int");
            if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
            {
                theSpCode.Append(@"                                 , @theUserAccountID int ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
            {
                theSpCode.Append(@"                                 , @theWorkstationID int  ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	                                , @processName nvarchar(255)");
            }

            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
                {
                    theSpCode.Append(@"	                        SET @theUserAccountID = (SELECT UserID FROM dbo.LogUser where UserSid = @theUserAccountSid)");
                }
                if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
                {
                    theSpCode.Append(@"                     	SET @theWorkstationID = (SELECT ComputerID FROM dbo.LogComputer where EPSGUID = @WorkstationId
																															AND ComputerName = @theWorkstationName    )");
                }
            }
            else if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
                {
                    theSpCode.Append(@"	                        SET @theUserAccountID = (SELECT UserID FROM [ActivityLog].[User] where UserSid = @theUserAccountSid)");
                }
                if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
                {
                    theSpCode.Append(@"                     	SET @theWorkstationID = (SELECT ComputerID FROM [ActivityLog].[Computer] where ComputerId = @WorkstationId
																															AND ComputerName = @theWorkstationName    )");
                }
            }
            
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	                        SET @processName = @theProcessName;
																								");
            }
            
            theSpCode.Append(@"                                                             
															---------------------------------------------------------------
															BEGIN
																if object_id('tempdb..#DeletedEntries') IS NOT NULL
																	DROP TABLE #DeletedEntries;
			
																CREATE TABLE #DeletedEntries (EntryID BIGINT NOT NULL PRIMARY KEY CLUSTERED);
																
																BEGIN
																	BEGIN TRY
																		INSERT  #DeletedEntries
																	    SELECT  distinct top (@batchSize)
																			    LE.EntryID
                            ");
            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                theSpCode.Append(@"												    FROM 	dbo.LogEntry as LE (NOLOCK)");
                if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
                {
                    theSpCode.Append(@"	                                        join [dbo].[LogEntry_To_LogData] as LTL on LE.EntryID = LTL.EntryID
																	        join [dbo].[LogData] as LD on (LTL.DataID = LD.DataID AND LTL.DataTypeID = 12)");
                }
            }
            else if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                theSpCode.Append(@"												    FROM 	[ActivityLog].[Entry] as LE (NOLOCK)");
                if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
                {
                    theSpCode.Append(@"	                                        join [ActivityLog].[Entry_Data] as LTL on LE.EntryID = LTL.EntryID
																	        join [ActivityLog].[Data] as LD on (LTL.DataID = LD.DataID AND LTL.DataTypeID = 12)");
                }
            }
            theSpCode.Append(@"                                          WHERE   LE.ActionID NOT IN (");
            theSpCode.Append(theLiveData.DbSqlSpControllerData.EventsToExclude);
            theSpCode.Append("                                                                     ) ");
            if (theLiveData.DbSqlSpControllerData.CutOffDays)
            {
                theSpCode.Append(@"                                      AND     LE.UTCDateTIME < @cutOffDate ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
            {
                theSpCode.Append(@"	                                     AND	 LE.UserID = @theUserAccountID ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
            {
                theSpCode.Append(@"	                                     AND     LE.ComputerID = @theWorkStationID ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	                                     AND     LD.Value = @processName; ");
            }
            theSpCode.Append(@"                                     SET  @numberOfRecordsProcessed = (@@ROWCOUNT);
														                DELETE  LTL
                                                                        ");
            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                theSpCode.Append(@"
														                FROM  dbo.LogEntry_To_LogData LTL
														                INNER JOIN  #DeletedEntries DE
															                ON  LTL.EntryID = DE.EntryID;
	
														                SET NOCOUNT OFF;
														                DELETE  LE
														                FROM  dbo.LogEntry LE
														                INNER JOIN  #DeletedEntries DE
															                ON  LE.EntryID = DE.EntryID;
                                                                        ");
            }
            else if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                theSpCode.Append(@"
														                FROM  [ActivityLog].[Entry_Data] LTL
														                INNER JOIN  #DeletedEntries DE
															                ON  LTL.EntryID = DE.EntryID;
	
														                SET NOCOUNT OFF;
														                DELETE  LE
														                FROM  [ActivityLog].[Entry] LE
														                INNER JOIN  #DeletedEntries DE
															                ON  LE.EntryID = DE.EntryID;
                                                                        ");
            }
            theSpCode.Append(@"						                SET NOCOUNT ON;

														                TRUNCATE TABLE #DeletedEntries;
													                END TRY
													                BEGIN CATCH
													                END CATCH;
												                    DROP TABLE #DeletedEntries   
	
												                    SET NOCOUNT OFF;
                                                                    ");
            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                theSpCode.Append(@"
												                    IF EXISTS (SELECT * FROM dbo.LogData (NOLOCK) WHERE DataID NOT IN (SELECT DataID FROM LogEntry_To_LogData (NOLOCK)))
													                    BEGIN
														                    BEGIN TRY
															                    DELETE  TOP (@batchSize)
															                    FROM  dbo.LogData
                                                                                WHERE  DataID NOT IN (SELECT DataID FROM LogEntry_To_LogData (NOLOCK));
														                    ");
            }
            else if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                theSpCode.Append(@"
												                    IF EXISTS (SELECT * FROM [ActivityLog].[Data] (NOLOCK) WHERE DataID NOT IN (SELECT DataID FROM [ActivityLog].[Entry_Data] (NOLOCK)))
													                    BEGIN
														                    BEGIN TRY
															                    DELETE  TOP (@batchSize)
															                    FROM  [ActivityLog].[Data]
                                                                                WHERE  DataID NOT IN (SELECT DataID FROM [ActivityLog].[Entry_Data] (NOLOCK));
														                    ");
            }
            theSpCode.Append(@"							                    END TRY
														                    BEGIN CATCH
														                    END CATCH;
													                    END; -- IF EXISTS LogData
                                                                    RETURN @numberOfRecordsProcessed;
                                                            END;
											            END;
                                                    END;
											            

										            ");
            return theSpCode.ToString();
        }

        private static string BuildTotalRecordCalcSqlString(string storedProcedureName, DbSqlSpController theLiveData)
        {
            StringBuilder theSpCode = new StringBuilder();
            bool atLeastOneParamAlready = false;
            theSpCode.Append(@"CREATE PROCEDURE [dbo].[" + storedProcedureName + @"]
								");
            if (theLiveData.DbSqlSpControllerData.CutOffDays || theLiveData.DbSqlSpControllerData.SelectedUser != "everyone" || theLiveData.DbSqlSpControllerData.SelectedComputer != "all" || theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	    (");
            }

            if (theLiveData.DbSqlSpControllerData.CutOffDays)
            {
                theSpCode.Append(@"     @cutOffDate datetime");
                atLeastOneParamAlready = true;
            }
            if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
            {
                if (atLeastOneParamAlready)
                {
                    theSpCode.Append(@"	    , @theUserAccountSid nvarchar(200)");
                }
                else
                {
                    theSpCode.Append(@"	    @theUserAccountSid nvarchar(200)");
                    atLeastOneParamAlready = true;
                }

            }
            if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
            {
                if (atLeastOneParamAlready)
                {
                    theSpCode.Append(@"	    , @WorkstationId nvarchar(255)
									, @theWorkstationName nvarchar(255)");
                }
                else
                {
                    theSpCode.Append(@"	    @WorkstationId nvarchar(255)
									, @theWorkstationName nvarchar(255)");
                    atLeastOneParamAlready = true;
                }
            }
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                if (atLeastOneParamAlready)
                {
                    theSpCode.Append(@"	    ,@theProcessName nvarchar(255)");
                }
                else
                {
                    theSpCode.Append(@"	    @theProcessName nvarchar(255)");
                    atLeastOneParamAlready = true;
                }
            }
            if (theLiveData.DbSqlSpControllerData.CutOffDays || theLiveData.DbSqlSpControllerData.SelectedUser != "everyone" || theLiveData.DbSqlSpControllerData.SelectedComputer != "all" || theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	    )");
            }

            theSpCode.Append(@"	    
													AS
													BEGIN
														DECLARE @recordCount bigint");
            if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
            {
                theSpCode.Append(@"                                 , @theUserAccountID int ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
            {
                theSpCode.Append(@"                                 , @theWorkstationID int  ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	                                , @processName nvarchar(255)");
            }
            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
                {
                    theSpCode.Append(@"	                        SET @theUserAccountID = (SELECT UserID FROM dbo.LogUser where UserSid = @theUserAccountSid)");
                }
                if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
                {
                    theSpCode.Append(@"                     	SET @theWorkstationID = (SELECT ComputerID FROM dbo.LogComputer where EPSGUID = @WorkstationId
																														AND ComputerName = @theWorkstationName    )");
                }
            }
            else if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
                {
                    theSpCode.Append(@"	                        SET @theUserAccountID = (SELECT UserID FROM [ActivityLog].[User] where UserSid = @theUserAccountSid)");
                }
                if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
                {
                    theSpCode.Append(@"                     	SET @theWorkstationID = (SELECT ComputerID FROM [ActivityLog].[Computer] where ComputerId = @WorkstationId
																														AND ComputerName = @theWorkstationName    )");
                }
            }
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	                        SET @processName = @theProcessName;
																							");
            }
            
            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                theSpCode.Append(@"                             ---------------------------------------------------------------
														SET @recordCount = (SELECT COUNT_BIG(*) FROM dbo.LogEntry As LE (NOLOCK)");
                if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
                {
                    theSpCode.Append(@"	                                                            join [dbo].[LogEntry_To_LogData] as LTL on LE.EntryID = LTL.EntryID
																							join [dbo].[LogData] as LD on (LTL.DataID = LD.DataID AND LTL.DataTypeID = 12)");
                }
            }
            else if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                theSpCode.Append(@"                             ---------------------------------------------------------------
														SET @recordCount = (SELECT COUNT_BIG(*) FROM [ActivityLog].[Entry] As LE (NOLOCK)");
                if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
                {
                    theSpCode.Append(@"	                                                            join [ActivityLog].[Entry_Data] as LTL on LE.EntryID = LTL.EntryID
																							join [ActivityLog].[Data] as LD on (LTL.DataID = LD.DataID AND LTL.DataTypeID = 12)");
                }
            }
            theSpCode.Append(@"                                                 WHERE   LE.ActionID NOT IN (");
            theSpCode.Append(theLiveData.DbSqlSpControllerData.EventsToExclude);
            theSpCode.Append("                                                                     ) ");
            if (theLiveData.DbSqlSpControllerData.CutOffDays)
            {
                theSpCode.Append(@"                                             AND     LE.UTCDateTIME < @cutOffDate ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedUser != "everyone")
            {
                theSpCode.Append(@"	                                            AND	 LE.UserID = @theUserAccountID ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedComputer != "all")
            {
                theSpCode.Append(@"	                                            AND     LE.ComputerID = @theWorkStationID ");
            }
            if (theLiveData.DbSqlSpControllerData.SelectedProcess != "all")
            {
                theSpCode.Append(@"	                                            AND     LD.Value = @processName ");
            }
            theSpCode.Append(@"                                                 )
														RETURN @recordCount
													END");
            return theSpCode.ToString();
            
        }

        private static string DeleteExistingMaintenanceStoredProcedures(DbSqlSpController theLiveData)
        {
            string result = "failure";
            result = DropTheStoredProcedure(theLiveData.DbSqlSpControllerData.RecordDeletionStoredProcedureName, theLiveData);
            if (result == "success")
            {
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " " + theLiveData.DbSqlSpControllerData.RecordDeletionStoredProcedureName + " Stored Procedure dropped.");
                result = CheckForStoredProcedure(theLiveData.DbSqlSpControllerData.RecordDeletionStoredProcedureName, theLiveData);
                if (result == "success")
                {
                    LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " " + theLiveData.DbSqlSpControllerData.RecordDeletionStoredProcedureName + " Stored Procedure confirmed deleted.");
                    result = DropTheStoredProcedure(theLiveData.DbSqlSpControllerData.TotalRecordsCalcStoredProcedureName, theLiveData);
                    if (result == "success")
                    {
                        LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " " + theLiveData.DbSqlSpControllerData.TotalRecordsCalcStoredProcedureName + " Stored Procedure dropped.");
                        result = CheckForStoredProcedure(theLiveData.DbSqlSpControllerData.TotalRecordsCalcStoredProcedureName, theLiveData);
                        if (result == "success")
                        {
                            LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " " + theLiveData.DbSqlSpControllerData.TotalRecordsCalcStoredProcedureName + " Stored Procedure confirmed deleted.");
                        }
                    }
                }
            }
            return result;
        }

        private static string CheckForStoredProcedure(string storedProcedureName, DbSqlSpController theLiveData)
        {
            string result = "failure";
            string checkForSpCode = @"  IF NOT EXISTS(SELECT 1 FROM sys.procedures 
												WHERE Name = '" + storedProcedureName + @"')
										BEGIN
											PRINT 'none found' 
										END";
            SqlConnection sqlDbConnection = new SqlConnection(theLiveData.DbSqlSpControllerData.SqlConnectionString);
            sqlDbConnection.InfoMessage += new SqlInfoMessageEventHandler(theLiveData.myConnection_InfoMessage);
            try
            {
                sqlDbConnection.Open();
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " SQL connection for " + storedProcedureName + "Stored Procedure Check OPEN.");
                SqlCommand spCreateCommand = new SqlCommand(checkForSpCode, sqlDbConnection);
                spCreateCommand.ExecuteNonQuery();
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " SQL command for " + storedProcedureName + " Stored Procedure Check finished.");
                sqlDbConnection.Close();
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, ex.Message.ToString());
                result = " Error checking for Stored Procedure:" + ex.Message.ToString();
            }
            if (theLiveData.DbSqlSpControllerData.SpCheckReturnString.Contains("none found"))
            {
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " SQL check for Stored Procedure returned:" + theLiveData.DbSqlSpControllerData.SpCheckReturnString.ToString());
                result = "success";
            }
            else
            {
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " Error: " + storedProcedureName + " Stored Procedure still exists after Dropping.");
                result = theLiveData.DbSqlSpControllerData.SpCheckReturnString.ToString();
            }
            return result;
        }

        private static string DropTheStoredProcedure(string storedProcedureName, DbSqlSpController theLiveData)
        {
            string result = "failure";
            string dropSpCode = @"  IF EXISTS(SELECT 1 FROM sys.procedures 
												WHERE Name = '" + storedProcedureName + @"')
										BEGIN
											DROP PROCEDURE dbo." + storedProcedureName + @" 
										END";
            SqlConnection sqlDbConnection = new SqlConnection(theLiveData.DbSqlSpControllerData.SqlConnectionString);
            try
            {
                sqlDbConnection.Open();
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " SQL connection for " + storedProcedureName + " Stored Procedure Dropping OPEN.");
                SqlCommand spCreateCommand = new SqlCommand(dropSpCode, sqlDbConnection);
                spCreateCommand.ExecuteNonQuery();
                LoggingClass.SaveEventToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, " SQL command for " + storedProcedureName + " Stored Procedure Drop completed.");
                sqlDbConnection.Close();
                result = "success";
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(theLiveData.DbSqlSpControllerData.LogFileLocation, ex.Message.ToString());
                result = " Error with dropping the " + storedProcedureName + " Stored Procedure:" + ex.Message.ToString();
            }
            return result;
        }


    }
}
