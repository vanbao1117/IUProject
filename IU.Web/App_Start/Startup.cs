using IU.Web.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace IU.Web
{
    [ExcludeFromCodeCoverage]
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });

            #region Mobile: Generate token & authenticate by token
            //Mobile: generate token (Mode email)
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/authtoken"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(360),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);


            // Mobile: authenticate by token with each request
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            #endregion


            // Make long polling connections wait a maximum of 110 seconds for a
            // response. When that time expires, trigger a timeout command and
            // make the client reconnect.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

            // Wait a maximum of 180 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(180);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(60);

            GlobalHost.Configuration.DefaultMessageBufferSize = 500;

            app.MapSignalR();
        }
    }
}