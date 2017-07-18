using System;

namespace Lumension_Advanced_DB_Maintenance.Data
{
    public class SqlDeletionEventargs : EventArgs
    {
        private int _recordsDeletedThisBatch;

        public int RecordsDeletedThisBatch
        {
            get { return _recordsDeletedThisBatch; }
            set { _recordsDeletedThisBatch = value; }
        }

    }
}
