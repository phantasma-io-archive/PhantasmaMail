using System.Threading.Tasks;

namespace PhantasmaMail.Services.Authentication
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated { get; }

        User AuthenticatedUser { get; }

        Task<bool> LoginWithUsername(string username, string password);

        Task<bool> LoginAsync(string encryptedKey, string password);

        Task<bool> LoginAsync(string wif);

        Task LogoutAsync();
    }
}
