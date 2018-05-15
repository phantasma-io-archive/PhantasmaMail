using System;
using System.Collections.Generic;
using System.Text;
using PhantasmaMail.Models;

namespace PhantasmaMail.Services.Db
{
    public interface IPhantasmaDb
    {
        IEnumerable<Message> GetMessages();

        bool AddMessage(Message message);
    }
}
