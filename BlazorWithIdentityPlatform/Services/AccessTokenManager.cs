using Microsoft.Identity.Client;

namespace BlazorWithIdentityPlatform.Services
{
    public class AccessTokenManager
    {
        private readonly string _clientId;
        private readonly string _tenantId;
        private readonly string[] _scopes;
        private readonly IPublicClientApplication _app;

        public AccessTokenManager(string clientId, string tenantId, string[] scopes)
        {
            _clientId = clientId;
            _tenantId = tenantId;
            _scopes = scopes;
            var authority = "https://login.microsoftonline.com/" + _tenantId;
            _app = PublicClientApplicationBuilder.Create(_clientId)
            .WithAuthority(authority)
            .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
            .Build();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var accounts = await _app.GetAccountsAsync();
                var result = await _app.AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                return result.AccessToken;
            }
            catch (MsalUiRequiredException)
            {
                // No silent token request possible, need to ask user to sign-in interactively
                var result = await _app.AcquireTokenInteractive(_scopes)
                    .ExecuteAsync();

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error acquiring access token: {ex.Message}");
                return null;
            }
        }
    }
}
