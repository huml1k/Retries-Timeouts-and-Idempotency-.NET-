namespace APIGateway.Routes
{
    public class RouteManager
    {
        public Dictionary<string, string> Routes { get; } = new();

        public RouteManager(IConfiguration configuration) 
        {
            var routes = configuration.GetSection("Routes").GetChildren();
            foreach (var route in routes) 
            {
                Routes.Add(route.Key, route.Value);
            }
        }
    }
}
