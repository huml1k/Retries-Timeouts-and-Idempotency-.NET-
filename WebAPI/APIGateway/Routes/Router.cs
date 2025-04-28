using APIGateway.Parser;
using APIGateway.Routes.Model;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace APIGateway.Routes
{
    public class Router
    {
        public List<Routes.Model.Routes> Routes { get; set; } 
        public Destination AuthenticationDestination { get; set; } 

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RouteManager _routeManager;

        public Router(RouteManager routeManager, IHttpClientFactory httpClientFactory)
        {
            _routeManager = routeManager;
            _httpClientFactory = httpClientFactory;
            LoadRoutes("./routes.json");
        }

        // Router.cs
        private void LoadRoutes(string routeConfigFilePath)
        {
            try
            {
                if (!File.Exists(routeConfigFilePath))
                    throw new FileNotFoundException($"Файл конфигурации {routeConfigFilePath} не найден.");

                var json = File.ReadAllText(routeConfigFilePath);
                dynamic routerConfig = JsonConvert.DeserializeObject(json);

                Routes = JsonConvert.DeserializeObject<List<Routes.Model.Routes>>(Convert.ToString(routerConfig.routes));

                foreach (var route in Routes)
                {
                    route.Destination = new Destination(
                        route.Destination.Uri,
                        route.Destination.RequiresAuthentication,
                        _httpClientFactory
                    );
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка загрузки маршрутов.", ex);
            }
        }

        public async Task<HttpResponseMessage> RouteRequest(HttpRequest request)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[1];

            Destination destination;
            try
            {
                destination = Routes.First(r => r.Endpoint.Equals(basePath)).Destination;
            }
            catch
            {
                return ConstructErrorMessage("Такого пути не существует");
            }

            if (destination.RequiresAuthentication)
            {
                string token = request.Headers["token"];
                var authResponse = await AuthenticationDestination.SendRequest(request);
                if (!authResponse.IsSuccessStatusCode)
                    return ConstructErrorMessage("Authentication failed.");
            }

            return await destination.SendRequest(request);
        }

        private HttpResponseMessage ConstructErrorMessage(string error)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(error)
            };
        }
    }
}