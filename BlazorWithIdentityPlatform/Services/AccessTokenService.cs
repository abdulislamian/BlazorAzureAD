using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorWithIdentityPlatform.Services
{
    public class AccessTokenService
    {
        private readonly IConfidentialClientApplication _app;
        private readonly HttpClient _httpClient;

        public AccessTokenService(IConfidentialClientApplication app, HttpClient httpClient)
        {
            _app = app;
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            try
            {
                //var authorizationCode = HttpContext.Request.Query["code"];
                var result = await _app.AcquireTokenForClient(scopes).ExecuteAsync();
                return result.AccessToken;
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                throw new Exception("Invalid scope for the application");
            }
        }

        public async Task<string> CallGraphApiAsync()
        {
            var token = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to call Graph API: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(content);

            return user.DisplayName;
        }
    }

    public class User
    {
        public string DisplayName { get; set; }
        // Add other properties as needed
    }
}
