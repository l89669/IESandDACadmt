using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IESandDACadmt.Model
{
    public enum SqlQueryReturnType
    {
        DataTable,
        String,
        Integer
    }

    public class singleSqlHealthQuery
    {
        private string _queryName;

        public string QueryName
        {
            get { return _queryName; }
            set { _queryName = value; }
        }

        private string _queryCode;

        public string QueryCode
        {
            get { return _queryCode; }
            set { _queryCode = value; }
        }

        private SqlQueryReturnType _returnType;

        public SqlQueryReturnType ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }

        private string _queryResult;

        public string QueryResult
        {
            get { return _queryResult; }
            set { _queryResult = value; }
        }

        private List<string> _possibleSqlRoles;

        public List<string> PossibleSqlRoles
        {
            get { return _possibleSqlRoles; }
            set { _possibleSqlRoles = value; }
        }

        private bool _queryRanOk;

        public bool QueryRanOk
        {
            get { return _queryRanOk; }
            set { _queryRanOk = value; }
        }

        private bool _sqlRoleCheckNeeded;

        public bool SqlRoleCheckNeeded
        {
            get { return _sqlRoleCheckNeeded; }
            set { _sqlRoleCheckNeeded = value; }
        }


    }
}
