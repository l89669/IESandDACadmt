using System;
using System.Data;

namespace Lumension_Advanced_DB_Maintenance.Forms
{
    public class SqlQueryEventArgs : EventArgs
    {
        public DataTable SqlQueryResults { get; set; }
    }
}
