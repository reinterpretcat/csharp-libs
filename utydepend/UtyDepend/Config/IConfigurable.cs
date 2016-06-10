namespace UtyDepend.Config
{
    /// <summary> Defines configurable class behaviour. </summary>
    public interface IConfigurable
    {
        /// <summary> Configures object using configuration section. </summary>
        /// <param name="configSection">Configuration section.</param>
        void Configure(IConfigSection configSection);
    }
}