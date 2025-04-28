namespace APIGateway.Routes
{
    public class RouteManager
    {
        public Dictionary<string, string> Routes { get; } = new();

        public RouteManager(IConfiguration configuration)
        {
            var routesSection = configuration.GetSection("Routes");
            if (routesSection == null)
                throw new ArgumentNullException("Конфигурация 'Routes' не найдена.");

            foreach (var route in routesSection.GetChildren())
                Routes.Add(route.Key, route.Value);
        }
    }
}
