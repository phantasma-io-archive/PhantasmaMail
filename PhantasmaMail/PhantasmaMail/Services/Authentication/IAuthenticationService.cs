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

		Task<bool> LoginAsync(string email, string password); 

		Task<bool> LoginWithMicrosoftAsync();

		Task<bool> UserIsAuthenticatedAndValidAsync();

		Task LogoutAsync();
	}
}
