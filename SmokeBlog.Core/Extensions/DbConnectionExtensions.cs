using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SmokeBlog.Core.Extensions
{
    public static class DbConnectionExtensions
    {
        public static bool Exist(this IDbConnection conn, string sql, object para)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
IF EXISTS ({0})
BEGIN
    SELECT CONVERT(BIT,1);
END
ELSE
BEGIN
    SELECT CONVERT(BIT,0);
END
", sql);

            return conn.ExecuteScalar<bool>(sb.ToString(), para);
        }
    }
}
