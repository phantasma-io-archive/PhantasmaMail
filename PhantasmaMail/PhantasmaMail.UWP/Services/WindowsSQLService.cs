using System.IO;
using Windows.Storage;
using PhantasmaMail.Services.Db;
using PhantasmaMail.UWP.Services;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(WindowsSQLService))]

namespace PhantasmaMail.UWP.Services
{
    public class WindowsSQLService : ISQLite
    {
        SQLiteConnection ISQLite.GetConnection()
        {
            var sqliteFilename = "Phantasma.sqlite";
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}