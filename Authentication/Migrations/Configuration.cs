namespace Authentication.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Authentication.Infraestructure.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            SetSqlGenerator("MySql.Data.MySqlClient",
                new MySql.Data.Entity.MySqlMigrationSqlGenerator());

            SetHistoryContextFactory("MySql.Data.MySqlClient",
            (conn, schema) => new MySqlHistoryContext(conn, schema));
        }

        protected override void Seed(Authentication.Infraestructure.AuthContext context)
        {

        }
    }
}
