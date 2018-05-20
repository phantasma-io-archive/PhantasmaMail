using SQLite;

namespace PhantasmaMail.Services.Db
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}
