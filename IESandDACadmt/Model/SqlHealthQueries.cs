using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IESandDACadmt.ViewModel;

namespace IESandDACadmt.Model
{
    class SqlHealthQueries
    {
        private DbSqlSpController _liveSqlDbSPController;

        public SqlHealthQueries(DbSqlSpController theLiveSqlDbSpController)
        {
            _liveSqlDbSPController = theLiveSqlDbSpController;
            BuildSqlServerconfigQueryList();
            BuildSqlWaitStatsQueryList();
            BuildSqlStoredProcWaitStatsQueryList();
            BuildSqlLogTableIndexStatsQueryList();
            BuildSqlLogTableStatisticsQueryList();
        }

        private void BuildSqlLogTableStatisticsQueryList()
        {
            StringBuilder runtimeString = new StringBuilder();
            runtimeString.Append(@"
            DECLARE @MyCursor CURSOR;
	        DECLARE @IndexName nvarchar(150),
			        @LogTableName nvarchar(100),
			        @DatabaseId int,
			        @IndexId int,
			        @IndexDid int,
			        @Object_Id int,
			        @RowCnt bigint,
			        @RowModCnt bigint,
			        @Percentage decimal(5,2),
			        @Recommendation bit;
	        -- 0. Set up required data items
	        CREATE TABLE #statsChecks(
						        SchemaName nvarchar(128),
								TableName nvarchar(100),
						        IndexName nvarchar(150),
						        LastStatUpdateTime DateTime,
						        RowsCount BigInt,
						        ChangesSinceLastStatUpdate bigint,
						        PercentageOfWayToNextStatsUpdate decimal(5,2),
						        StatsRecommendation bit,
						        );
	        SET @DatabaseId = (SELECT db_id('" + _liveSqlDbSPController.DbSqlSpControllerData.DataBaseName + @"'));
	        INSERT INTO #statsChecks
			        (
					        TableName,
					        IndexName
			        )
			        SELECT  sys.objects.name tableName,
					        sys.indexes.name indexName
			        FROM    sys.indexes
			        JOIN    sys.objects ON sys.indexes.object_id = sys.objects.object_id
			        WHERE   sys.objects.type_desc = 'USER_TABLE'
			        ORDER BY 1 asc;
	
	        -- 3. Check the last update times for each index and Check #changes since last update for each index
	        USE " + _liveSqlDbSPController.DbSqlSpControllerData.DataBaseName + @"
	        BEGIN
		        SET @MyCursor = CURSOR FOR
		        select IndexName,TableName from #statsChecks
		        OPEN @MyCursor 
		        FETCH NEXT FROM @MyCursor 
		        INTO @IndexName, @LogTableName

		        WHILE @@FETCH_STATUS = 0
		        BEGIN
			        SET @IndexId = (SELECT object_id FROM sys.indexes WHERE name = @IndexName AND OBJECT_NAME(object_id) = @LogTableName);
			        SET @IndexDid = (SELECT indid FROM sys.sysindexes WHERE name = @IndexName AND OBJECT_NAME(id) = @LogTableName);
			        UPDATE #statsChecks
			        SET SchemaName = (select TABLE_SCHEMA from INFORMATION_SCHEMA.tables WHERE TABLE_NAME = @LogTableName),
						LastStatUpdateTime = (SELECT STATS_DATE(@IndexId, @IndexDid)),
				        RowsCount = CAST((SELECT rowcnt FROM sys.sysindexes WHERE name = @IndexName AND OBJECT_NAME(id) = @LogTableName) as BigInt),
				        ChangesSinceLastStatUpdate = CAST((SELECT rowmodctr FROM sys.sysindexes WHERE name = @IndexName AND OBJECT_NAME(id) = @LogTableName) as BigInt)
			        WHERE IndexName = @IndexName
                    AND     TableName = @LogTableName;
			        FETCH NEXT FROM @MyCursor 
			        INTO @IndexName , @LogTableName
		        END; 
	        END;
	        -- 4. Calc % of way to next Stats Update trigger (find DB setting for the trigger value)
	        BEGIN
		        SET @MyCursor = CURSOR FOR
		        select IndexName, TableName, RowsCount, ChangesSinceLastStatUpdate from #statsChecks
		        OPEN @MyCursor 
		        FETCH NEXT FROM @MyCursor 
		        INTO @IndexName, @LogTableName, @RowCnt, @RowModCnt

		        WHILE @@FETCH_STATUS = 0
		        BEGIN
			        IF @RowCnt > 500
				        SET @Percentage = CAST(((@RowModCnt/((@RowCnt * 0.2) + 500))*100) as decimal(5,2));
			        ELSE
				        SET @Percentage = 0.0;
			        UPDATE #statsChecks
				        SET PercentageOfWayToNextStatsUpdate = @Percentage
				        WHERE IndexName = @IndexName
                    AND     TableName = @LogTableName;;
			        IF @Percentage >= 90.0
				        UPDATE #statsChecks SET StatsRecommendation = 1 WHERE IndexName = @IndexName AND     TableName = @LogTableName;
			        ELSE
				        UPDATE #statsChecks SET StatsRecommendation = 0 WHERE IndexName = @IndexName AND     TableName = @LogTableName;
			        FETCH NEXT FROM @MyCursor 
			        INTO @IndexName, @LogTableName, @RowCnt, @RowModCnt 
		        END; 
	        END;
	        -- 6. Print out temp table details to screen
	        SELECT TOP(100) * FROM #statsChecks
	        WHERE RowsCount > 1000
	        ORDER BY PercentageOfWayToNextStatsUpdate desc
	        -- 9. Clean up
	        CLOSE @MyCursor ;
	        DEALLOCATE @MyCursor;
	        DROP TABLE #statsChecks;
            ");
            _sqlLogTableStatisticsQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "Log Table Statistics",
                QueryCode = runtimeString.ToString(),
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.DataTable,
                SqlRoleCheckNeeded = true
            });
        }

        private void BuildSqlLogTableIndexStatsQueryList()
        {
            StringBuilder runtimeString = new StringBuilder();
            runtimeString.Append(@"
                    DECLARE @MyCursor CURSOR;
	                DECLARE @IndexName nvarchar(150),
			                @LogTableName nvarchar(100),
			                @DatabaseId int,
			                @IndexId int,
			                @IndexDid int,
			                @Object_Id int,
			                @RowCnt bigint,
			                @RowModCnt bigint,
			                @Percentage decimal(5,2),
			                @Recommendation bit;
	                -- 0. Set up required data items
	                CREATE TABLE #statsChecks(
						                SchemaName nvarchar(128),
								        TableName nvarchar(100),
						                IndexName nvarchar(150),
						                LastStatUpdateTime DateTime,
						                RowsCount BigInt,
						                Percentage tinyint,
						                Rebuild bit,
						                Reorganize bit
						                );
	                SET @DatabaseId = (SELECT db_id('" + _liveSqlDbSPController.DbSqlSpControllerData.DataBaseName + @"'));

                    INSERT INTO #statsChecks
			                (
					                TableName,
					                IndexName
			                )
			                SELECT  sys.objects.name tableName,
					                sys.indexes.name indexName
			                FROM    sys.indexes
			                JOIN    sys.objects ON sys.indexes.object_id = sys.objects.object_id
			                WHERE   sys.objects.type_desc = 'USER_TABLE'
			                ORDER BY 1 asc;
	
	                -- 3. Check the last update times for each index and Check #changes since last update for each index
	                BEGIN
		                SET @MyCursor = CURSOR FOR
		                select IndexName, TableName from #statsChecks
		                OPEN @MyCursor 
		                FETCH NEXT FROM @MyCursor 
		                INTO @IndexName, @LogTableName

		                WHILE @@FETCH_STATUS = 0
		                BEGIN
			                SET @IndexId = (SELECT object_id FROM sys.indexes WHERE name = @IndexName AND OBJECT_NAME(object_id) = @LogTableName);
			                SET @IndexDid = (SELECT indid FROM sys.sysindexes WHERE name = @IndexName AND OBJECT_NAME(id) = @LogTableName);
			                UPDATE #statsChecks
			                SET SchemaName = (select TABLE_SCHEMA from INFORMATION_SCHEMA.tables WHERE TABLE_NAME = @LogTableName),
						        RowsCount = CAST((SELECT rowcnt FROM sys.sysindexes WHERE name = @IndexName AND OBJECT_NAME(id) = @LogTableName) as BigInt)
			                WHERE IndexName = @IndexName
                            AND     TableName = @LogTableName;
			                FETCH NEXT FROM @MyCursor 
			                INTO @IndexName , @LogTableName
		                END; 
	                END;
	                -- 5 Check Index Fragmentation Levels
	                BEGIN
		                SET @MyCursor = CURSOR FOR
		                select TableName, IndexName from #statsChecks
		                OPEN @MyCursor 
		                FETCH NEXT FROM @MyCursor 
		                INTO @LogTableName, @IndexName

		                WHILE @@FETCH_STATUS = 0
		                BEGIN
			                SET @Object_Id = (OBJECT_ID(@LogTableName));
			                SET @IndexId = (SELECT index_id FROM sys.indexes WHERE name = @IndexName AND OBJECT_NAME(object_id) = @LogTableName);
			                BEGIN TRY
				                SET @Percentage = CAST((SELECT MAX(avg_fragmentation_in_percent) 
												                FROM sys.dm_db_index_physical_stats(@DatabaseId,@Object_Id, @IndexId, NULL , NULL)) as decimal(5,2));
			                END TRY
			                BEGIN CATCH
				                SET @Percentage = 0.0
			                END CATCH
			                if @Percentage > 30.0
				                BEGIN
				                UPDATE #statsChecks
				                SET	Percentage = @Percentage,
					                Rebuild = 1,
					                Reorganize = 0
				                WHERE	TableName = @LogTableName
				                AND		IndexName = @IndexName
				                END
			                else
				                BEGIN
				                if @Percentage > 5.0
					                BEGIN
					                UPDATE #statsChecks
					                SET	Percentage = @Percentage,
						                Reorganize = 1,
						                Rebuild = 0
					                WHERE	TableName = @LogTableName
					                AND		IndexName = @IndexName
					                END
				                else
					                BEGIN
					                UPDATE #statsChecks
					                SET	Percentage = @Percentage,
						                Rebuild = 0,
						                Reorganize = 0
					                WHERE	TableName = @LogTableName
					                AND		IndexName = @IndexName
					                END
				                END
		
			                FETCH NEXT FROM @MyCursor 
			                INTO @LogTableName, @IndexName 
		                END; 
	                END;
	                -- 6. Print out temp table details to screen
	                SELECT TOP(100) * FROM #statsChecks
	                WHERE RowsCount > 1000
	                ORDER BY Percentage desc
	                -- 9. Clean up
	                CLOSE @MyCursor ;
	                DEALLOCATE @MyCursor;
	                DROP TABLE #statsChecks;
            ");
            _sqlHealthLogTableIndexStatsQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "Log Table Index Stats",
                QueryCode = runtimeString.ToString(),
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.DataTable,
                SqlRoleCheckNeeded = true
            });
        }

        private void BuildSqlStoredProcWaitStatsQueryList()
        {
            StringBuilder runtimeString = new StringBuilder();
            runtimeString.Append(@"
	                SELECT DB_NAME(database_id) As DBname,
	                OBJECT_NAME(object_id) as SPName,
	                cached_time,
	                last_execution_time,
	                execution_count,
	                last_elapsed_time,
	                min_elapsed_time,
	                max_elapsed_time,
	                (total_elapsed_time/execution_count) as Average_Execution_Time,
	                min_physical_reads,
	                max_physical_reads,
	                (total_physical_reads/execution_count) as Average_Physical_Reads,
	                min_logical_writes,
	                max_logical_writes,
	                (total_logical_reads/execution_count) as Average_Logical_Writes
	                FROM sys.dm_exec_procedure_stats
	                ");
            if (_liveSqlDbSPController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.EMSS)
            {
                runtimeString.Append(@" WHERE database_id IN (DB_ID('UPCCommon'), DB_ID('PLUS')) ");
            }
            else if (_liveSqlDbSPController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.ES)
            {
                runtimeString.Append(@" WHERE database_id = (DB_ID('" + _liveSqlDbSPController.DbSqlSpControllerData.DataBaseName + @"')) ");
            }
            runtimeString.Append(@"
	                ORDER BY DBname asc, Average_Execution_Time desc
            ");
            _sqlHealthStoredProcedureStatsQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "Stored Procedure Wait Stats",
                QueryCode = runtimeString.ToString(),
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.DataTable,
                SqlRoleCheckNeeded = true
            });
        }

        private void BuildSqlWaitStatsQueryList()
        {
            _sqlHealthWaitStatsQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "SQL Server Wait Stats",
                QueryCode = @"
            Begin
	            WITH [Waits] AS
		            (SELECT
			            [wait_type],
			            [wait_time_ms] / 1000.0 AS [WaitS],
			            ([wait_time_ms] - [signal_wait_time_ms]) / 1000.0 AS [ResourceS],
			            [signal_wait_time_ms] / 1000.0 AS [SignalS],
			            [waiting_tasks_count] AS [WaitCount],
		               100.0 * [wait_time_ms] / SUM ([wait_time_ms]) OVER() AS [Percentage],
			            ROW_NUMBER() OVER(ORDER BY [wait_time_ms] DESC) AS [RowNum]
		            FROM sys.dm_os_wait_stats
		            WHERE [wait_type] NOT IN (
			            N'BROKER_EVENTHANDLER', N'BROKER_RECEIVE_WAITFOR',
			            N'BROKER_TASK_STOP', N'BROKER_TO_FLUSH',
			            N'BROKER_TRANSMITTER', N'CHECKPOINT_QUEUE',
			            N'CHKPT', N'CLR_AUTO_EVENT',
			            N'CLR_MANUAL_EVENT', N'CLR_SEMAPHORE',
 
			            -- Maybe uncomment these four if you have mirroring issues
			            N'DBMIRROR_DBM_EVENT', N'DBMIRROR_EVENTS_QUEUE',
			            N'DBMIRROR_WORKER_QUEUE', N'DBMIRRORING_CMD',
 
			            N'DIRTY_PAGE_POLL', N'DISPATCHER_QUEUE_SEMAPHORE',
			            N'EXECSYNC', N'FSAGENT',
			            N'FT_IFTS_SCHEDULER_IDLE_WAIT', N'FT_IFTSHC_MUTEX',
 
			            -- Maybe uncomment these six if you have AG issues
			            N'HADR_CLUSAPI_CALL', N'HADR_FILESTREAM_IOMGR_IOCOMPLETION',
			            N'HADR_LOGCAPTURE_WAIT', N'HADR_NOTIFICATION_DEQUEUE',
			            N'HADR_TIMER_TASK', N'HADR_WORK_QUEUE',
 
			            N'KSOURCE_WAKEUP', N'LAZYWRITER_SLEEP',
			            N'LOGMGR_QUEUE', N'MEMORY_ALLOCATION_EXT',
			            N'ONDEMAND_TASK_QUEUE',
			            N'PREEMPTIVE_XE_GETTARGETSTATE',
			            N'PWAIT_ALL_COMPONENTS_INITIALIZED',
			            N'PWAIT_DIRECTLOGCONSUMER_GETNEXT',
			            N'QDS_PERSIST_TASK_MAIN_LOOP_SLEEP', N'QDS_ASYNC_QUEUE',
			            N'QDS_CLEANUP_STALE_QUERIES_TASK_MAIN_LOOP_SLEEP',
			            N'QDS_SHUTDOWN_QUEUE', N'REDO_THREAD_PENDING_WORK',
			            N'REQUEST_FOR_DEADLOCK_SEARCH', N'RESOURCE_QUEUE',
			            N'SERVER_IDLE_CHECK', N'SLEEP_BPOOL_FLUSH',
			            N'SLEEP_DBSTARTUP', N'SLEEP_DCOMSTARTUP',
			            N'SLEEP_MASTERDBREADY', N'SLEEP_MASTERMDREADY',
			            N'SLEEP_MASTERUPGRADED', N'SLEEP_MSDBSTARTUP',
			            N'SLEEP_SYSTEMTASK', N'SLEEP_TASK',
			            N'SLEEP_TEMPDBSTARTUP', N'SNI_HTTP_ACCEPT',
			            N'SP_SERVER_DIAGNOSTICS_SLEEP', N'SQLTRACE_BUFFER_FLUSH',
			            N'SQLTRACE_INCREMENTAL_FLUSH_SLEEP',
			            N'SQLTRACE_WAIT_ENTRIES', N'WAIT_FOR_RESULTS',
			            N'WAITFOR', N'WAITFOR_TASKSHUTDOWN',
			            N'WAIT_XTP_RECOVERY',
			            N'WAIT_XTP_HOST_WAIT', N'WAIT_XTP_OFFLINE_CKPT_NEW_LOG',
			            N'WAIT_XTP_CKPT_CLOSE', N'XE_DISPATCHER_JOIN',
			            N'XE_DISPATCHER_WAIT', N'XE_TIMER_EVENT')
		            AND [waiting_tasks_count] > 0
		            )
	            SELECT
		            MAX ([W1].[wait_type]) AS [WaitType],
		            CAST (MAX ([W1].[WaitS]) AS DECIMAL (16,2)) AS [Wait_S],
		            CAST (MAX ([W1].[ResourceS]) AS DECIMAL (16,2)) AS [Resource_S],
		            CAST (MAX ([W1].[SignalS]) AS DECIMAL (16,2)) AS [Signal_S],
		            MAX ([W1].[WaitCount]) AS [WaitCount],
		            CAST (MAX ([W1].[Percentage]) AS DECIMAL (5,2)) AS [Percentage],
		            CAST ((MAX ([W1].[WaitS]) / MAX ([W1].[WaitCount])) AS DECIMAL (16,4)) AS [AvgWait_S],
		            CAST ((MAX ([W1].[ResourceS]) / MAX ([W1].[WaitCount])) AS DECIMAL (16,4)) AS [AvgRes_S],
		            CAST ((MAX ([W1].[SignalS]) / MAX ([W1].[WaitCount])) AS DECIMAL (16,4)) AS [AvgSig_S],
		            CAST ('https://www.sqlskills.com/help/waits/' + MAX ([W1].[wait_type]) as XML) AS [Help/Info URL]
	            FROM [Waits] AS [W1]
	            INNER JOIN [Waits] AS [W2]
		            ON [W2].[RowNum] <= [W1].[RowNum]
	            GROUP BY [W1].[RowNum]
	            HAVING SUM ([W2].[Percentage]) - MAX( [W1].[Percentage] ) < 95; -- percentage threshold
            END",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.DataTable,
                SqlRoleCheckNeeded = true
            });
        }

        private void BuildSqlServerconfigQueryList()
        {
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "SQL Server Name",
                QueryCode = @"SELECT @@SERVERNAME",
                PossibleSqlRoles = new List<string> { "sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "SQL Instance Name",
                QueryCode = @"SELECT @@SERVICENAME",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "SQL Version",
                QueryCode = @"SELECT @@VERSION",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "CPU Counts",
                QueryCode = @"select N'Logical CPU :' +  RTRIM(CAST((SELECT cpu_count FROM sys.dm_os_sys_info) as nvarchar(4))) + N' = '
                                                                            + N'Hyperthread Ratio (' + RTRIM(CAST((SELECT hyperthread_ratio FROM sys.dm_os_sys_info) as nvarchar(4))) + N') X '
                                                                            + N'Physical CPU Count (' + RTRIM(CAST((SELECT cpu_count / hyperthread_ratio FROM sys.dm_os_sys_info) as nvarchar(4))) +N')'; ",
                PossibleSqlRoles = new List<string> {"sysadmin" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "Max Memory",
                QueryCode = @"EXEC sp_configure 'show advanced options', 1
                                RECONFIGURE
                                DECLARE @result nvarchar(255);
                                SET @result =  CAST((SELECT value_in_use FROM sys.configurations WHERE name LIKE '%max server memory%') as nvarchar(60))  + N'MB'
		                        EXEC sp_configure 'show advanced options', 0
                                RECONFIGURE;
                                SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "System Memory",
                QueryCode = @"EXEC sp_configure 'show advanced options', 1
                                RECONFIGURE
                                DECLARE @result nvarchar(255);
                                SET @result =  RTRIM(CAST((SELECT total_physical_memory_kb/1024 FROM sys.dm_os_sys_memory) as nvarchar(60))) + N'MB'   --This one requires the RECONFIGURE
		                        EXEC sp_configure 'show advanced options', 0
                                RECONFIGURE;
                                SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "System Type",
                QueryCode = @"DECLARE @result nvarchar(255);
                                SET @result = (SELECT CASE
                                    WHEN virtual_machine_type = 1 THEN 'Virtual'   
							        ELSE 'Physical'   
						        END
                              FROM sys.dm_os_sys_info);
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "Server Uptime",
                QueryCode = @"DECLARE @crdate DATETIME, @hr VARCHAR(50), @min VARCHAR(5)
                                SELECT @crdate = crdate FROM sysdatabases WHERE NAME = 'tempdb'
                                SELECT @hr = (DATEDIFF(mi, @crdate, GETDATE())) / 60
                                IF((DATEDIFF ( mi, @crdate, GETDATE()))/60)=0
				                        SELECT @min = (DATEDIFF(mi, @crdate, GETDATE()))
                                ELSE
                                    SELECT @min=(DATEDIFF(mi, @crdate, GETDATE()))-((DATEDIFF(mi, @crdate, GETDATE()))/60)*60
		                        select @hr + 'hrs ' + @min + 'mins';",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "MaxDop",
                QueryCode = @"EXEC sp_configure 'show advanced options', 1
		                        DECLARE @result nvarchar(255);
                                RECONFIGURE
		                        SET @result = CAST((SELECT value FROM sys.configurations WHERE name = 'max degree of parallelism') as nvarchar(255))
                                EXEC sp_configure 'show advanced options', 0
                                RECONFIGURE
                                SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "Cost Of Parallelism",
                QueryCode = @"EXEC sp_configure 'show advanced options', 1
		                        DECLARE @result nvarchar(255);
                                RECONFIGURE
		                        SET @result = CAST((SELECT value FROM sys.configurations WHERE name = 'cost threshold for parallelism') as nvarchar(255))
                                EXEC sp_configure 'show advanced options', 0
                                RECONFIGURE
                                SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = "SQL Server Collation",
                QueryCode = @"SELECT CAST((select serverproperty('collation')) as nvarchar(255));",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            if (_liveSqlDbSPController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.EMSS)
            {
                ReadDatabaseDetails("PLUS", _sqlHealthConfigQueryList);
                ReadDatabaseDetails("UPCCommon", _sqlHealthConfigQueryList);
                ReadPolicyVersion(_liveSqlDbSPController.DbSqlSpControllerData.DataBaseName, _sqlHealthConfigQueryList, "AntivirusPolicyVersion", "AntiVirus");
                ReadPolicyVersion(_liveSqlDbSPController.DbSqlSpControllerData.DataBaseName, _sqlHealthConfigQueryList, "ApplicationControlPolicyVersion", "Application Control");
                ReadPolicyVersion(_liveSqlDbSPController.DbSqlSpControllerData.DataBaseName, _sqlHealthConfigQueryList, "DeviceControlPolicyVersion", "Device Control");
            }
            else if (_liveSqlDbSPController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.ES)
            {
                ReadDatabaseDetails(_liveSqlDbSPController.DbSqlSpControllerData.DataBaseName, _sqlHealthConfigQueryList);
                _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
                {
                    QueryName = "Policy USN",
                    QueryCode = @"DECLARE @result nvarchar(255);
                              SET @result = (SELECT MAX(USN) FROM dbo.object_log);
                              SELECT @result",
                    PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                    ReturnType = SqlQueryReturnType.String,
                    SqlRoleCheckNeeded = true
                });
            }
        }

        private List<singleSqlHealthQuery> _sqlHealthConfigQueryList = new List<singleSqlHealthQuery>();

        public List<singleSqlHealthQuery> SqlHealthConfigQueryList
        {
            get { return _sqlHealthConfigQueryList; }
            set { _sqlHealthConfigQueryList = value; }
        }

        private void ReadDatabaseDetails(string theDatabaseName, List<singleSqlHealthQuery> theQueryList)
        {
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + " Compatibility Level",
                QueryCode = @"DECLARE @result nvarchar(255);
                                SET @result = CAST((SELECT compatibility_level FROM sys.databases WHERE name = '" + theDatabaseName + @"') as nvarchar(255)); 
                                SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + " Collation",
                QueryCode = @"DECLARE @result nvarchar(255);
                                SET @result = CAST((SELECT collation_name FROM sys.databases WHERE name = '" + theDatabaseName + @"') as nvarchar(255)); 
                                SELECT @result",
                PossibleSqlRoles = new List<string> { "sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + " Recovery Model",
                QueryCode = @"DECLARE @result nvarchar(255);
                                SET @result = CAST((SELECT recovery_model_desc FROM sys.databases WHERE name = '" + theDatabaseName + @"') as nvarchar(255)); 
                                SELECT @result",
                PossibleSqlRoles = new List<string> { "sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + " State",
                QueryCode = @"DECLARE @result nvarchar(255);
                                SET @result = CAST((SELECT state_desc FROM sys.databases WHERE name = '" + theDatabaseName + @"') as nvarchar(255)); 
                                SELECT @result",
                PossibleSqlRoles = new List<string> { "sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + " User Access Description",
                QueryCode = @"DECLARE @result nvarchar(255);
                                SET @result = CAST((SELECT user_access_desc FROM sys.databases WHERE name = '" + theDatabaseName + @"') as nvarchar(255)); 
                                SELECT @result",
                PossibleSqlRoles = new List<string> { "sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" MDF File Location",
                QueryCode = @"DECLARE @result nvarchar(255);
                              USE " + theDatabaseName + @"
                              SET @result = CAST((SELECT DISTINCT mf.physical_name PhysicalFileLocation

                                                            FROM sys.master_files mf CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs
                                                            WHERE DB_NAME(dovs.database_id) = '" + theDatabaseName + @"'
													        AND mf.physical_name LIKE '%.MDF') as nvarchar(260));
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" MDF File Size",
                QueryCode = @"DECLARE @result nvarchar(255);
                              SET @result = CAST((SELECT SUM(size / 128.0) FROM sys.database_files WHERE[type_desc] = 'ROWS') as nvarchar(30)) + N' MB';
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" MDF File Internal-Free-Space",
                QueryCode = @"DECLARE @result nvarchar(255);
                              SET @result = CAST((SELECT SUM(size / 128.0 - CAST(FILEPROPERTY(name, 'SpaceUsed') AS INT) / 128.0) FROM sys.database_files WHERE[type_desc] = 'ROWS') as nvarchar(30)) + N' MB';             
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" MDF File Drive-Free-Space",
                QueryCode = @"DECLARE @result nvarchar(255);
                              SET @result = CAST((SELECT DISTINCT CONVERT(INT, dovs.available_bytes / 1048576.0) AS FreeSpaceInMB
                              FROM sys.master_files mf CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs
                              WHERE DB_NAME(dovs.database_id) = '" + theDatabaseName + @"'
                              AND mf.physical_name LIKE '%.MDF') as nvarchar(30)) + N' MB';
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" LDF File Location",
                QueryCode = @"DECLARE @result nvarchar(255);
                              USE " + theDatabaseName + @"
                              SET @result = CAST((SELECT DISTINCT mf.physical_name PhysicalFileLocation
                                FROM sys.master_files mf CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs
                                WHERE DB_NAME(dovs.database_id) = '" + theDatabaseName + @"'
                                AND mf.physical_name LIKE '%.LDF') as nvarchar(260));
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" LDF File Size",
                QueryCode = @"DECLARE @result nvarchar(255);
                              SET @result = CAST((SELECT SUM(size / 128.0) FROM sys.database_files WHERE[type_desc] = 'LOG') as nvarchar(30)) + N' MB';
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" LDF File Internal-Free-Space",
                QueryCode = @"DECLARE @result nvarchar(255);
                              SET @result = CAST((SELECT SUM(size / 128.0 - CAST(FILEPROPERTY(name, 'SpaceUsed') AS INT) / 128.0) FROM sys.database_files WHERE[type_desc] = 'LOG') as nvarchar(30)) + N' MB';
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = theDatabaseName + @" LDF File Drive-Free-Space",
                QueryCode = @"DECLARE @result nvarchar(255);
                               SET @result = CAST((SELECT DISTINCT CONVERT(INT, dovs.available_bytes / 1048576.0) AS FreeSpaceInMB
                                FROM sys.master_files mf CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs
                                WHERE DB_NAME(dovs.database_id) = '" + theDatabaseName + @"'
                                AND mf.physical_name LIKE '%.LDF') as nvarchar(30)) + N' MB';
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin"},
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });
        }

        private void ReadPolicyVersion(string theDatabaseName, List<singleSqlHealthQuery> theQueryList, string theSqlPolicyIdentifier, string thePolicyType)
        {
            _sqlHealthConfigQueryList.Add(new singleSqlHealthQuery
            {
                QueryName = thePolicyType + " Module Policy Version",
                QueryCode = @"DECLARE @result nvarchar(255);
                              USE " + theDatabaseName + @"
                              IF(SELECT[EPMPolicyItemDefault] FROM [" + theDatabaseName + @"].[dbo].[EndPointModulePolicyItem] WHERE EPMPolicyItemName LIKE '" + theSqlPolicyIdentifier + @"') > 0
			                    BEGIN
                                    SET @result = CAST((SELECT[EPMPolicyItemDefault] FROM [UPCCommon].[dbo].[EndPointModulePolicyItem] WHERE EPMPolicyItemName LIKE '" + theSqlPolicyIdentifier + @"') as nvarchar(20));	
            			        END
                              SELECT @result",
                PossibleSqlRoles = new List<string> {"sysadmin", "db_owner" },
                ReturnType = SqlQueryReturnType.String,
                SqlRoleCheckNeeded = true
            });

        }

        private List<singleSqlHealthQuery> _sqlHealthWaitStatsQueryList = new List<singleSqlHealthQuery>();

        public List<singleSqlHealthQuery> SqlHealthWaitStatsQueryList
        {
            get { return _sqlHealthWaitStatsQueryList; }
            set { _sqlHealthWaitStatsQueryList = value; }
        }

        private List<singleSqlHealthQuery> _sqlHealthStoredProcedureStatsQueryList = new List<singleSqlHealthQuery>();

        public List<singleSqlHealthQuery> SqlHealthStoredProcedureStatsQueryList
        {
            get { return _sqlHealthStoredProcedureStatsQueryList; }
            set { _sqlHealthStoredProcedureStatsQueryList = value; }
        }

        private List<singleSqlHealthQuery> _sqlHealthLogTableIndexStatsQueryList = new List<singleSqlHealthQuery>();

        public List<singleSqlHealthQuery> SqlHealthLogTableIndexStatsQueryList
        {
            get { return _sqlHealthLogTableIndexStatsQueryList; }
            set { _sqlHealthLogTableIndexStatsQueryList = value; }
        }

        private List<singleSqlHealthQuery> _sqlLogTableStatisticsQueryList = new List<singleSqlHealthQuery>();

        public List<singleSqlHealthQuery> SqlLogTableStatisticsQueryList
        {
            get { return _sqlLogTableStatisticsQueryList; }
            set { _sqlLogTableStatisticsQueryList = value; }
        }

    }
}
