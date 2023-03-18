using Npgsql;
using QueryPerformanceMaster.Domain.ConnectionSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryPerformanceMaster.Core.ConnectionProvider.PostgreSql
{
    public class PostgreSqlConnectionService : IPostgreSqlConnectionService
    {
        public PostreSqlConnectionSettings GetPostgreSqlConnectionSettings(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            return new PostreSqlConnectionSettings
            {
                Server = builder.Host,
                Port = builder.Port,
                MaxPoolSize= builder.MaxPoolSize,
                ConnectTimeout = builder.Timeout,
                Database= builder.Database,
                EnablePooling = builder.Pooling,
                Login = builder.Username,
                Password= builder.Password
            };
        }

        public string GetConnectionString(PostreSqlConnectionSettings settings)
        {
            var build = new NpgsqlConnectionStringBuilder
            {
                Host = settings.Server,
                ApplicationName = "QueryPerformanceMaster",
                TrustServerCertificate = true,
                Username = settings.Login,
                Password = settings.Password
            };

            if (settings.Database.Length > 0)
            {
                build.Database = settings.Database;
            }

            if (settings.ConnectTimeout != 0)
            {
                build.Timeout = settings.ConnectTimeout;
            }

            if (settings.MaxPoolSize != 0)
            {
                build.MaxPoolSize = settings.MaxPoolSize;
                build.MinPoolSize = settings.MaxPoolSize;
            }

            build.Pooling = settings.EnablePooling;

            return build.ConnectionString;
        }
    }
}
