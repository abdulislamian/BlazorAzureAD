using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.IdentityModel.Tokens.Jwt;

namespace BlazorWithIdentityPlatform.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var initialScopes = configuration["DownstreamApi:Scopes"]?.Split(' ') ?? configuration["MicrosoftGraph:Scopes"]?.Split(' ');
            //services.AddMicrosoftIdentityWebAppAuthentication(configuration)
            //    .EnableTokenAcquisitionToCallDownstreamApi(new[] { "Calendars.Read", "Calendars.ReadWrite" })
            //    .AddMicrosoftGraph(configuration.GetSection("MicrosoftGraph"))
            //    .AddInMemoryTokenCaches();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(configuration.GetSection("DownstreamApi"))
                .AddInMemoryTokenCaches();

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();

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
