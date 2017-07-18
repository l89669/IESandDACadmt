using System;
using System.Data;

namespace IESandDACadmt.Model
{
    public class SqlQueryEventArgs : EventArgs
    {
        public DataTable SqlQueryResults { get; set; }
    }
}
