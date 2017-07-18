using System;

namespace IESandDACadmt.Model
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
