using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Phantasma
{
    public interface IPhantasmaService
    {
        Task<string> GetMailboxFromAddress();

    }
}
