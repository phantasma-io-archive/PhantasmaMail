using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Phantasma
{
    public class PhantasmaMockService : IPhantasmaService
    {
        public Task<string> GetUserMailbox()
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterMailbox(string name)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendMessage(string destName, string hash)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMailCount(string boxName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMailContent(string name, int index)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllMails(string name, int count)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetMailsFromRange(string name, int first, int last)
        {
            throw new NotImplementedException();
        }
    }
}
