using System.Text;

namespace Lumension_Advanced_DB_Maintenance.BL
{
    public class SqlHealthReviewLogic
    {
        public enum SqlQueryType
        {
            ServerConfig = 1,
            AllWaitStats,
            SpWaitStats,
            IndexStats,
            TableStats,
        };

    }
}
