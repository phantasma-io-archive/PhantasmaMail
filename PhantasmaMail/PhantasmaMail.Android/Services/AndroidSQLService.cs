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
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "Phantasma.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}