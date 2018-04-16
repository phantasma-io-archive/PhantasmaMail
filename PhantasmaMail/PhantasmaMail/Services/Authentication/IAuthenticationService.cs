using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Authentication
{
	public interface IAuthenticationService // TODO: change to neo authenticate
	{
		bool IsAuthenticated { get; }

		//User AuthenticatedUser { get; }

		Task<bool> LoginAsync(string encryptedKey, string password);

	    Task<bool> LoginAsync(string wif);

		Task LogoutAsync();
	}
}
