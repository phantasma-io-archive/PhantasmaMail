using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Phantasma
{
    public interface IPhantasmaService
    {
        Task<string> GetUserMailbox();

        Task<string> RegisterMailbox(string name);

        Task<string> SendMessage(string destName, string hash);

        Task<int> GetMailCount(string boxName);

        Task<string> GetMailContent(string name, int index);

        Task<List<string>> GetAllMails(string name, int count);

        Task<List<string>> GetMailsFromRange(string name, int first, int last);
    }
}