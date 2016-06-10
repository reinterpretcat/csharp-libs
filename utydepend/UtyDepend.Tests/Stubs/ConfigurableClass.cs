using UtyDepend.Config;

namespace UtyDepend.Tests.Stubs
{
    class ConfigurableClass: IConfigurable
    {
        public IConfigSection ConfigSection { get; set; }
        public void Configure(IConfigSection config)
        {
            ConfigSection = config;
        }
    }
}
