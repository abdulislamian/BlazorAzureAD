using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace BlazorWithIdentityPlatform.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services,IConfiguration configuration)
        {

            var initialScopes = configuration["DownstreamApi:Scopes"]?.Split(' ') ?? configuration["MicrosoftGraph:Scopes"]?.Split(' ');
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(configuration.GetSection("MicrosoftGraph"))
                .AddInMemoryTokenCaches();
                services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

                return services; 
        }
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });

            return services;
        }
    }
}
