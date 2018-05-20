using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace PhantasmaMail.Services.Db
{
    public class PhantasmaDb : IPhantasmaDb
    {
        private readonly SQLiteAsyncConnection _connection;

        //CREATE
        public PhantasmaDb()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTableAsync<StoreMessage>();
        }

        //READ
        public async Task<IEnumerable<StoreMessage>> GetInboxMessages(string boxName)
        {
            var messages = await _connection.Table<StoreMessage>().Where(msg => msg.ToInbox == boxName).ToListAsync();
            return messages;
        }

        public async Task<IEnumerable<StoreMessage>> GetSentMessages(string boxName)
        {
            var messages = await _connection.Table<StoreMessage>().Where(msg => msg.FromInbox == boxName).ToListAsync();
            return messages;
        }

        //INSERT
        public async Task<bool> AddMessage(StoreMessage message)
        {
            if (await _connection.InsertAsync(message) > 0) return true;
            return false;
        }

        //DELETE
        public async Task<bool> DeleteMessage(StoreMessage message)
        {
            if (await _connection.DeleteAsync(message) > 0) return true;
            return false;
        }
    }
}