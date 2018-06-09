using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.RPC;

namespace PhantasmaMail.Services.Phantasma
{
    public interface IPhantasmaService
    {
        Task<string> GetMailboxFromAddress();

        Task<string> GetAddressFromMailbox(string boxName);

        Task<string> RegisterMailbox(string name);

        Task<string> UnregisterMailbox();

        Task<string> SendMessage(string destBoxName, string hash);

        Task<string> RemoveInboxMessage(int index);

        Task<string> RemoveOutboxMessage(int index);

        Task<int> GetInboxCount();

        Task<string> GetInboxContent(int index);

        Task<int> GetOutboxCount();

        Task<string> GetOutboxContent(int index);

        Task<List<string>> GetAllInboxMessages(int count);

        Task<List<string>> GetAllOutboxMessages(int count);

        Task<string> RemoveInboxMessages(int[] indexes);

        Task<string> RemoveOutboxMessages(int[] indexes);

        Task<string> EstimateMessageCost(string boxName, string message);

        NeoApiService ApiService { get; set; }
    }
}