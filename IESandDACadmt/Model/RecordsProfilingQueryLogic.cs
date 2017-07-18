namespace Lumension_Advanced_DB_Maintenance.BL
{
    public static class RecordsProfilingQueryLogic
	{
		public enum DataQueryType
		{
			ByDate = 1,
			ByType,
			ByComputer,
			ByUser,
			ByProcess,
			ByDevice
		};

	    public static string EmssByDateQuery { get; } = @"  SELECT  CAST( UTCDateTime as DATE ) As 'Date', 
			                                                    COUNT_BIG(*) As 'Count',
			                                                    MAX(DATEDIFF(DAY, le.UTCDateTime, GETDATE())) as 'AgeInDays',
			                                                    la.ActionName As 'ActionName'
	                                                    FROM    dbo.LogEntry AS le (NOLOCK)
		                                                    INNER JOIN dbo.LogAction la (NOLOCK) ON la.ActionID = le.ActionID
	                                                    GROUP BY CAST( UTCDateTime as DATE ), LA.ActionName
	                                                    ORDER BY 1 asc, LA.ActionName asc;";

	    public static string EmssByTypeQuery { get; } = @"  SELECT     COUNT_BIG(*) AS 'Count',
			                                                    la.ActionName As 'ActionName'
	                                                    FROM    dbo.LogEntry AS le (NOLOCK)
	                                                        INNER JOIN dbo.LogAction la (NOLOCK) ON la.ActionID = le.ActionID
	                                                    GROUP BY la.ActionName
	                                                    ORDER BY 1 desc";

	    public static string EmssByUserQuery { get; } = @"  SELECT TOP 20     COUNT_BIG(e.UserID)  AS 'Count', 
                                                                        e.Userid,
                                                                        MAX(u.NTUserName) As 'NTUserName'
	                                                    FROM dbo.LogEntry AS e
		                                                    INNER JOIN dbo.LogUser AS u	ON e.UserID = u.UserID
	                                                    GROUP BY e.UserID
	                                                    ORDER BY [Count] DESC;";

	    public static string EmssByComputerQuery { get; } = @"  SELECT TOP 20     COUNT_BIG(e.ComputerID) AS 'Count', 
                                                                            e.ComputerID,
                                                                            MAX(c.ComputerName) As 'ComputerName'
	                                                        FROM dbo.LogEntry AS e
		                                                        INNER JOIN dbo.LogComputer AS c ON e.ComputerID = c.ComputerID
	                                                        GROUP BY e.ComputerID
	                                                        ORDER BY [Count] DESC;";

	    public static string EmssByProcessQuery { get; } = @"	SELECT TOP 20   COUNT_BIG(d.Value) AS 'Count', 
                                                                            MAX(d.Value) AS 'ProcessName'
	                                                        FROM dbo.LogEntry_To_LogData AS ed
		                                                        JOIN dbo.LogData AS d ON (ed.DataID = d.DataID AND ed.DataTypeID = 12)
                                                            GROUP BY d.Value
	                                                        ORDER BY [Count] DESC;";

	    public static string EmssByDeviceQuery { get; } = @"	SELECT TOP 20   COUNT_BIG(d.Value) AS 'Count', 
                                                                            MAX(d.Value) AS 'DeviceName'
	                                                        FROM dbo.LogEntry_To_LogData AS ed
		                                                        JOIN dbo.LogData AS d ON (ed.DataID = d.DataID AND ed.DataTypeID = 8)
                                                            GROUP BY d.Value
	                                                        ORDER BY [Count] DESC;";

        public static string EsByDateQuery { get; } = @"  SELECT  CAST( UTCDateTime as DATE ) As 'Date', 
			                                                    COUNT_BIG(*) As 'Count',
			                                                    MAX(DATEDIFF(DAY, le.UTCDateTime, GETDATE())) as 'AgeInDays',
			                                                    la.ActionName As 'ActionName'
	                                                    FROM    [ActivityLog].[Entry] AS le (NOLOCK)
		                                                    INNER JOIN [ActivityLog].[Action] la (NOLOCK) ON la.ActionID = le.ActionID
	                                                    GROUP BY CAST( UTCDateTime as DATE ), LA.ActionName
	                                                    ORDER BY 1 asc, LA.ActionName asc;";

        public static string EsByTypeQuery { get; } = @"  SELECT     COUNT_BIG(*) AS 'Count',
			                                                    la.ActionName As 'ActionName'
	                                                    FROM    [ActivityLog].[Entry] AS le (NOLOCK)
	                                                        INNER JOIN [ActivityLog].[Action] la (NOLOCK) ON la.ActionID = le.ActionID
	                                                    GROUP BY la.ActionName
	                                                    ORDER BY 1 desc";

        public static string EsByUserQuery { get; } = @"  SELECT TOP 20     COUNT_BIG(e.UserID)  AS 'Count', 
                                                                         e.Userid,
                                                                        MAX(u.NTUserName) As 'NTUserName'
	                                                    FROM [ActivityLog].[Entry] AS e
		                                                    INNER JOIN [ActivityLog].[User] AS u	ON e.UserID = u.UserID
	                                                    GROUP BY e.UserID
	                                                    ORDER BY [Count] DESC;";

        public static string EsByComputerQuery { get; } = @"  SELECT TOP 20     COUNT_BIG(e.ComputerID) AS 'Count', 
                                                                             e.ComputerID,
                                                                            MAX(c.ComputerName) As 'ComputerName'
	                                                        FROM [ActivityLog].[Entry] AS e
		                                                        INNER JOIN [ActivityLog].[Computer] AS c ON e.ComputerID = c.ComputerID
	                                                        GROUP BY e.ComputerID
	                                                        ORDER BY [Count] DESC;";

        public static string EsByProcessQuery { get; } = @"	SELECT TOP 20   COUNT_BIG(d.Value) AS 'Count', 
                                                                            MAX(d.Value) AS 'ProcessName'
	                                                        FROM [ActivityLog].[Entry_Data] AS ed
		                                                        JOIN [ActivityLog].[Data] AS d ON (ed.DataID = d.DataID AND ed.DataTypeID = 12)
                                                            GROUP BY d.Value
	                                                        ORDER BY [Count] DESC;";

        public static string EsByDeviceQuery { get; } = @"	SELECT TOP 20   COUNT_BIG(d.Value) AS 'Count', 
                                                                            MAX(d.Value) AS 'DeviceName'
	                                                        FROM [ActivityLog].[Entry_Data] AS ed
		                                                        JOIN [ActivityLog].[Data] AS d ON (ed.DataID = d.DataID AND ed.DataTypeID = 8)
                                                            GROUP BY d.Value
	                                                        ORDER BY [Count] DESC;";
    }
}
