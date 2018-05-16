using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SingleRemoteAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme =
                    // FacebookDefaults.AuthenticationScheme;
                    // GoogleDefaults.AuthenticationScheme;
                    // MicrosoftAccountDefaults.AuthenticationScheme;
                    // OpenIdConnectDefaults.AuthenticationScheme;
                    // TwitterDefaults.AuthenticationScheme;
                    WsFederationDefaults.AuthenticationScheme;
            })
            // Any single remote provider can be substituted here.
            // See https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/other-logins for more providers
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins
            .AddFacebook(options =>
            {
                options.AppId = Configuration["facebook:appid"];
                options.AppSecret = Configuration["facebook:appsecret"];
            })
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["google:clientid"];
                options.ClientSecret = Configuration["google:clientsecret"];
            })
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = Configuration["microsoftaccount:clientid"];
                options.ClientSecret = Configuration["microsoftaccount:clientsecret"];
            })
            // https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapp-openidconnect-aspnetcore/
            .AddOpenIdConnect(options =>
            {
                options.ClientId = Configuration["oidc:clientid"];
                options.ClientSecret = Configuration["oidc:clientsecret"]; // for code flow
                options.Authority = Configuration["oidc:authority"];
            })
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/twitter-logins
            .AddTwitter(options =>
            {
                options.ConsumerKey = Configuration["twitter:consumerkey"];
                options.ConsumerSecret = Configuration["twitter:consumersecret"];
            })
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/ws-federation
            .AddWsFederation(options =>
            {
                options.Wtrealm = "https://Tratcheroutlook.onmicrosoft.com/WsFedSample";
                options.MetadataAddress = "https://login.windows.net/cdc690f9-b6b8-4023-813a-bae7143d1f87/FederationMetadata/2007-06/FederationMetadata.xml";
            })
            .AddCookie();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
