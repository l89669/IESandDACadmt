using System;
using System.Data;

namespace Lumension_Advanced_DB_Maintenance.Data
{
    public class SqlHealthReviewData
    {
        private DateTime _lastAnalysisRunTime;

        public DateTime LastAnalysisRunTime
        {
            get { return _lastAnalysisRunTime; }
            set { _lastAnalysisRunTime = value; }
        }

        private DataTable _serverConfigResults = new DataTable();

        public DataTable ServerConfigResults
        {
            get { return _serverConfigResults; }
            set { _serverConfigResults = value; }
        }

        private DataTable _waitStatsResults = new DataTable();

        public DataTable WaitStatsResults
        {
            get { return _waitStatsResults; }
            set { _waitStatsResults = value; }
        }

        private DataTable _spWaitStatsResults = new DataTable();

        public DataTable SpWaitStatsResults
        {
            get { return _spWaitStatsResults; }
            set { _spWaitStatsResults = value; }
        }

        private DataTable _logTableIndexStatsResults = new DataTable();

        public DataTable LogTableIndexStatsResults
        {
            get { return _logTableIndexStatsResults; }
            set { _logTableIndexStatsResults = value; }
        }

        private DataTable _logTablesStatisticsResults = new DataTable();

        public DataTable LogTableStatisticsResults
        {
            get { return _logTablesStatisticsResults; }
            set { _logTablesStatisticsResults = value; }
        }

        private volatile bool _isTabRefreshNeeded;

        public bool IsTabRefreshNeeded
        {
            get { return _isTabRefreshNeeded; }
            set { _isTabRefreshNeeded = value; }
        }


        private volatile int _tabIndex;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }


    }
}
