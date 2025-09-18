using Carter;

namespace BuildingBlocks.Extensions
{
    public static class CarterExtensions
    {
        public static void LoadAllModules(this CarterConfigurator configurator)
        {
            var modules = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(type => typeof(ICarterModule).IsAssignableFrom(type) && type.IsClass && !typeof(CarterModule).IsAssignableFrom(type));

            configurator.WithModules([..modules]);
        }
    }
}
