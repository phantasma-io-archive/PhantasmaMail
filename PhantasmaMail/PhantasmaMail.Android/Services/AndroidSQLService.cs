using System;
using System.IO;
using PhantasmaMail.Droid.Services;
using PhantasmaMail.Services.Db;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidSQLService))]
namespace PhantasmaMail.Droid.Services
{
    public class AndroidSQLService : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "Phantasma.sqlite";
            var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(dbPath, dbName);
            var conn = new SQLiteConnection(path);
            return conn;
        }

        public AndroidSQLService()
        {
            
        }
    }
}