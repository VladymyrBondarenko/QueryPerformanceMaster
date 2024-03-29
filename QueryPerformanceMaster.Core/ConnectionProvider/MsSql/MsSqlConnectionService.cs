﻿using System.Data.SqlClient;
using QueryPerformanceMaster.Domain.ConnectionSettings;
using QueryPerformanceMaster.App.Interfaces.ConnectionProvider;

namespace QueryPerformanceMaster.Core.ConnectionProvider.MsSql
{
    public class MsSqlConnectionService : IMsSqlConnectionService
    {
        public MsSqlConnectionSettings GetMsSqlConnectionSettings(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            return new MsSqlConnectionSettings
            {
                Server = builder.DataSource,
                IntegratedAuth = builder.IntegratedSecurity,
                ApplicationIntent = builder.ApplicationIntent,
                Login = builder.UserID,
                Password = builder.Password,
                Database = builder.InitialCatalog,
                ConnectTimeout = builder.ConnectTimeout,
                MaxPoolSize = builder.MaxPoolSize,
                EnablePooling = builder.Pooling
            };
        }

        public string GetConnectionString(MsSqlConnectionSettings settings)
        {
            var build = new SqlConnectionStringBuilder
            {
                DataSource = settings.Server,
                IntegratedSecurity = settings.IntegratedAuth,
                ApplicationName = "QueryPerformanceMaster",
                ApplicationIntent = settings.ApplicationIntent,
                TrustServerCertificate = true
            };
            if (!settings.IntegratedAuth && !settings.AzureMFA)
            {
                build.UserID = settings.Login;
                build.Password = settings.Password;
            }

            if (!string.IsNullOrWhiteSpace(settings.Database))
            {
                build.InitialCatalog = settings.Database;
            }

            if (settings.ConnectTimeout != 0)
            {
                build.ConnectTimeout = settings.ConnectTimeout;
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
