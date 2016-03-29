using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeradorDocumentacao.API
{
    public class Startup
    {


        public void Configuration(IAppBuilder app)
        {
            using (var config = new HttpConfiguration())
            {
                ConfigureWebApi(config);

                app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
                app.UseWebApi(config);
            }
        }

        public static void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Documentacao", action = "Gerar", id = RouteParameter.Optional });

            using (var cancelledTaskBugWorkaroundMessageHandler = new CancelledTaskBugWorkaroundMessageHandler())
            {
                //config.MessageHandlers.Add(cancelledTaskBugWorkaroundMessageHandler);
            }
        }
    }

    public class CancelledTaskBugWorkaroundMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            // Try to suppress response content when the cancellation token has fired; ASP.NET will log to the Application event log if there's content in this case.
            if (cancellationToken.IsCancellationRequested)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}
