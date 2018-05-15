using System.Collections.Generic;
using System.Linq;
using PhantasmaMail.Models;
using SQLite;
using Xamarin.Forms;

namespace PhantasmaMail.Services.Db
{
    public class PhantasmaDb : IPhantasmaDb
    {
        private readonly SQLiteConnection _connection;

        //CREATE
        public PhantasmaDb()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<Message>();
        }

        //READ
        public IEnumerable<Message> GetMessages()
        {
            var messages = from msg in _connection.Table<Message>() select msg;
            return messages.ToList();
        }

        //INSERT
        public bool AddMessage(Message message)
        {
            if (_connection.Insert(message) > 0) return true;
            return false;
        }

        //DELETE
        public bool DeleteMessage(int id)
        {
            if (_connection.Delete<Message>(id) > 0) return true;
            return false;
        }
    }
}