using System;
using System.IO;
using PhantasmaMail.iOS.Services;
using PhantasmaMail.Services.Db;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSSQLService))]
namespace PhantasmaMail.iOS.Services
{
    public class iOSSQLService : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "Phantasma.sqlite";
            var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var libraryPath = Path.Combine(dbPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, dbName);
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}