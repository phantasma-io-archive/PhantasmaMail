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
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "Phantasma.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}