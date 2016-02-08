using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Authentication.Infraestructure
{
    public class MySqlInitializer : IDatabaseInitializer<AuthContext>
    {
        public void InitializeDatabase(AuthContext context)
        {
            if (!context.Database.Exists())
            {
                context.Database.Create();
            }
            else
            {
                var migrationHistoryTableExists =
                ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                  @"SELECT COUNT(*) FROM information_schema.tables 
                  WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                  "dbc7def778f1594379835ea550009238aa"));

                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                }
            }
        }
    }
}