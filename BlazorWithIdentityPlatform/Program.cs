using BlazorWithIdentityPlatform.Components;
using BlazorWithIdentityPlatform.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Syncfusion.Blazor;
using System.Net.Http.Headers;
using static System.Formats.Asn1.AsnWriter;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF1cXmhLYVJ/WmFZfVpgdV9GYVZRR2YuP1ZhSXxXdkdiW39fcHVVRWhUUUY=");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSyncfusionBlazor();

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

async Task<string> test()
{
    //string[] scopes = new string[] { "user.read" };
    var clientID = builder.Configuration.GetSection("AzureAd").GetSection("ClientId");
    var secretID = builder.Configuration.GetSection("AzureAd").GetSection("ClientSecret");
    var tenantID = builder.Configuration.GetSection("AzureAd").GetSection("TenantId");
    using (var httpClient = new HttpClient())
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientID.Value },
            { "client_secret", secretID.Value },
            { "scope", "https://graph.microsoft.com/.default" }
        });

        var response = await httpClient.PostAsync(
            $"https://login.microsoftonline.com/common/oauth2/v2.0/token",
            content);

        //var tokenss = TokenAcquirerFactory.GetDefaultInstance<

        var responseContent = await response.Content.ReadAsStringAsync();
        var accessToken = responseContent.Split("\"access_token\":\"")[1].Split("\"")[0];

        return accessToken;
    }
}

builder.Services.AddRazorPages();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAntiforgery();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
