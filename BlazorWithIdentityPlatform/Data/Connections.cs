using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace BlazorWithIdentityPlatform.Data
{
    public static class Connections
    {
        private static IPublicClientApplication CreatePublicClient(string authority)
        {
            var appBuilder = PublicClientApplicationBuilder.Create("cee81331-b341-4875-aabf-f8c838a394b2") // DO NOT USE THIS CLIENT ID IN YOUR APP!!!! WE WILL DELETE IT!
                .WithAuthority(authority)
                .WithRedirectUri("http://localhost"); // make sure to register this redirect URI for the interactive login to work

            var app = appBuilder.Build();
            Console.WriteLine($"Built public client");

            return app;
        }
        public static async Task<string> GenerateToken()
        {
            var pca = CreatePublicClient("https://login.microsoftonline.com/common");
            var cacheHelper = await CreateCacheHelperAsync().ConfigureAwait(false);
            cacheHelper.RegisterCache(pca.UserTokenCache);

            // Advanced scenario for when 2 or more apps share the same cache
            cacheHelper.CacheChanged += (s, e) => // this event is very expensive perf wise
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"Cache Changed, Added: {e.AccountsAdded.Count()} Removed: {e.AccountsRemoved.Count()}");
                Console.ResetColor();
            };

            try
            {
                // Initialize MSAL PublicClientApplication with your application/client ID
                //var pca = CreatePublicClient("https://login.microsoftonline.com/common");

                // Define the scopes required for accessing calendar events
                var scopes = new[] { "User.Read", "Calendars.ReadWrite" };

                AuthenticationResult result;
                try
                {
                    //string accessToken = await TokenAcquisitionService.GetAccessTokenForUserAsync(scopes);
                    // Acquire an access token for the user silently (without prompting for login if possible)
                    var accounts = await pca.GetAccountsAsync().ConfigureAwait(false);
                    result = await pca.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                    .ExecuteAsync()
                                    .ConfigureAwait(false);
                    return result.AccessToken;
                }
                catch (MsalUiRequiredException)
                {
                    // User interaction is required to acquire a token.
                    // Redirect the user to the sign-in page or prompt them to log in again.
                    result = await pca.AcquireTokenInteractive(scopes)
                        .ExecuteAsync().ConfigureAwait(false);
                    if (result.AccessToken != null)
                    {
                        cacheHelper.RegisterCache(pca.UserTokenCache);
                        var accounts = await pca.GetAccountsAsync().ConfigureAwait(false);
                    }
                }
                return string.IsNullOrEmpty(result.AccessToken) ? result.AccessToken : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        private static async Task<MsalCacheHelper> CreateCacheHelperAsync()
        {
            StorageCreationProperties storageProperties;

            try
            {
                storageProperties = new StorageCreationPropertiesBuilder(
                    "cacheToken.txt",
                    MsalCacheHelper.UserRootDirectory) // "cee81331-b341-4875-aabf-f8c838a394b2"
                                                       // .WithLinuxKeyring(
                                                       //     Config.LinuxKeyRingSchema,
                                                       //     Config.LinuxKeyRingCollection,
                                                       //     Config.LinuxKeyRingLabel,
                                                       //     Config.LinuxKeyRingAttr1,
                                                       //     Config.LinuxKeyRingAttr2)
                                                       // .WithMacKeyChain(
                                                       //     Config.KeyChainServiceName,
                                                       //     Config.KeyChainAccountName)
                .WithCacheChangedEvent( // do NOT use unless really necessary, high perf penalty!
                   "cee81331-b341-4875-aabf-f8c838a394b2",
                "https://login.microsoftonline.com/common")
                .Build();

                var cacheHelper = await MsalCacheHelper.CreateAsync(
                    storageProperties).ConfigureAwait(false);

                cacheHelper.VerifyPersistence();
                return cacheHelper;

            }
            catch (MsalCachePersistenceException e)
            {
                Console.WriteLine($"WARNING! Unable to encrypt tokens at rest." +
                    $" Saving tokens in plaintext at {Path.Combine(MsalCacheHelper.UserRootDirectory, "cacheToken.txt")} ! Please protect this directory or delete the file after use");
                Console.WriteLine($"Encryption exception: " + e);

                // clientId : "cee81331-b341-4875-aabf-f8c838a394b2"
                storageProperties =
                    new StorageCreationPropertiesBuilder(
                        "cacheToken.txt" + ".plaintext", // do not use the same file name so as not to overwrite the encypted version
                        MsalCacheHelper.UserRootDirectory).WithUnprotectedFile().Build();

                var cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(false);
                cacheHelper.VerifyPersistence();

                return cacheHelper;
            }
        }
    }

}
