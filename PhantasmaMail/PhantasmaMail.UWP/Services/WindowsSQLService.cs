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
        public SQLiteAsyncConnection GetConnection()
        {

            string documentPath = ApplicationData.Current.LocalFolder.Path;
            string path = Path.Combine(documentPath, "Phantasma.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}