using APIBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;

namespace APIBrowser
{
    public class ApiHelper
    {
        static IDbConnection connection = Init();

        internal static IDbConnection Init()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            connection.Execute(@"
                IF NOT EXISTS (SELECT * FROM sys.tables
                WHERE name = N'API' AND type = 'U')
                BEGIN
                    CREATE TABLE [API] ([key] NVARCHAR(512) NOT NULL, [parameters] NVARCHAR(2048) NOT NULL);
                    CREATE TABLE [DATA] ([key] NVARCHAR(512) NOT NULL, [data] NVARCHAR(2048) NOT NULL);
                END
            ");

            return connection;
        }

        internal static void SaveApi(Api api)
        {
            connection.Execute("DELETE FROM [API] WHERE [key] = @key", new { key = api.Key });
            connection.Execute("INSERT INTO [API] ([key], [parameters]) VALUES (@key, @parameters);", new { key = api.Key, parameters = api.Parameters });
        }

        internal static void Persist(string key, NameValueCollection collection)
        {
            var pars = connection.Query<string>("SELECT [parameters] FROM [API] where [key] = @key", new { key = key }).FirstOrDefault();
            if (pars != null)
            {
                var p = pars.Split(' ');
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ", " + string.Join(" ", p.Select(k => string.Format("{0}={1}", k, collection[k])));
                connection.Execute("INSERT INTO [DATA] ([key], [data]) VALUES (@key, @data)", new { key = key, data = data });
            }
        }

        internal static IEnumerable<string> Get(string key)
        {
            return connection.Query<string>("SELECT [data] FROM [DATA] WHERE [key] = @key", new { key = key });
        }

        internal static IEnumerable<string> GetAllKeys()
        {
            return connection.Query<string>("SELECT DISTINCT [key] from [API]");
        }
    }
}