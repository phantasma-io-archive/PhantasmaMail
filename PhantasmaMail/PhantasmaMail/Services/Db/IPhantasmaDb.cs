using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Db
{
    public interface IPhantasmaDb
    {
        Task<IEnumerable<StoreMessage>> GetInboxMessages(string boxName);

        Task<IEnumerable<StoreMessage>> GetSentMessages(string boxName);

        Task<bool> AddMessage (StoreMessage message);

        Task<bool> DeleteMessage(StoreMessage message);
    }
}
