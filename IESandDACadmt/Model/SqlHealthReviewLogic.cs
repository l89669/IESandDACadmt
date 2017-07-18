using System.Text;

namespace IESandDACadmt.Model
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
