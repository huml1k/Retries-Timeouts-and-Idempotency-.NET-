using APIGateway.Parser;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace APIGateway.Routes
{
    public class Router
    {
        public List<Route> Routes { get; set; }
        public Destionation AuthenticationService { get; set; }

        public Router(string routeConfigFilePath) 
        {
            var router = JsonParser.LoadFromFile<dynamic>(routeConfigFilePath);

            Routes = JsonParser.Deserialize<List<Route>>(Convert.ToString(router.routes));

            AuthenticationService = JsonParser.Deserialize(Convert.ToString(router.authenticationService));
        }

        public async Task<HttpResponseMessage> RouteRequest(HttpRequest request)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[1];

            Destionation destination;
            try
            {
                destination = Routes.First(r => r. Equals(basePath)).Defaults;
            }
            catch
            {
                return ConstructErrorMessage("The path could not be found.");
            }

            if (destination.RequiresAuthentication)
            {
                string token = request.Headers["token"];
                request.Query.Append(new KeyValuePair<string, StringValues>("token", new StringValues(token)));
                HttpResponseMessage authResponse = await AuthenticationService.SendRequest(request);
                if (!authResponse.IsSuccessStatusCode) return ConstructErrorMessage("Authentication failed.");
            }

            return await destination.SendRequest(request);
        }

        private HttpResponseMessage ConstructErrorMessage(string error)
        {
            HttpResponseMessage errorMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(error)
            };
            return errorMessage;
        }

    }
}
