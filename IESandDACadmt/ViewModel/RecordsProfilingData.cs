using System.Collections.Generic;
using System.Data;

namespace Lumension_Advanced_DB_Maintenance.Data
{
    public class RecordsProfilingData
    {
        private DataTable _byDateDataRecords = new DataTable();

        public DataTable ByDateDataRecords
        {
            get { return _byDateDataRecords; }
            set { _byDateDataRecords = value; }
        }

        private DataTable _byTypeDataRecords = new DataTable();

        public DataTable ByTypeDataRecords
        {
            get { return _byTypeDataRecords; }
            set { _byTypeDataRecords = value; }
        }

        private DataTable _byUserDataRecords = new DataTable();

        public DataTable ByUserDataRecords
        {
            get { return _byUserDataRecords; }
            set { _byUserDataRecords = value; }
        }

        private DataTable _byComputerDataRecords = new DataTable();

        public DataTable ByComputerDataRecords
        {
            get { return _byComputerDataRecords; }
            set { _byComputerDataRecords = value; }
        }

        private DataTable _byProcessDataRecords = new DataTable();

        public DataTable ByProcessDataRecords
        {
            get { return _byProcessDataRecords; }
            set { _byProcessDataRecords = value; }
        }

        private DataTable _byDeviceDataRecords = new DataTable();

        public DataTable ByDeviceDataRecords
        {
            get { return _byDeviceDataRecords; }
            set { _byDeviceDataRecords = value; }
        }

        private Dictionary<string, double> _filteredChartData = new Dictionary<string, double>();

        public Dictionary<string, double> FilteredChartData
        {
            get { return _filteredChartData; }
            set { _filteredChartData = value; }
        }

        public RecordsProfilingData()
        {
            _byDateDataRecords.Columns.Add("Date", typeof(string));
            _byDateDataRecords.Columns.Add("Count", typeof(int));
            _byDateDataRecords.Columns.Add("AgeInDays", typeof(int));
            _byDateDataRecords.Columns.Add("ActionName", typeof(string));
        }

    }
}
