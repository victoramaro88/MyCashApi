using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.Cors;
using System.Net.Http.Headers;
using System.Web.Cors;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(MyCashApi.App_Start.Startup))]
namespace MyCashApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "api/{controller}/{id}",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ConfigureCors(app);

            AtivandoAcessTokens(app);

            app.UseWebApi(config);
        }

        private void AtivandoAcessTokens(IAppBuilder app)
        {
            var opcoesConfiguracaoToken = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                Provider = new ProviderDeTokensDeAcesso()
            };

            app.UseOAuthAuthorizationServer(opcoesConfiguracaoToken);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureCors(IAppBuilder app)
        {
            var politica = new CorsPolicy();

            politica.AllowAnyHeader = true;
            //politica.Origins.Add("*");
            politica.AllowAnyOrigin = true;
            //politica.Origins.Add("http://localhost:4200");
            politica.Methods.Add("GET");
            politica.Methods.Add("POST");

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(politica)
                }
            };
            app.UseCors(corsOptions);
        }
    }
}

//AUTENTICAÇÃO VIA TOKEN -> https://www.youtube.com/watch?v=S2Ryvw7ou3k