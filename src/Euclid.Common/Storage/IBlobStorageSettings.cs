using Euclid.Common.Configuration;
using Euclid.Common.Logging;
namespace Euclid.Common.Storage
{
    public interface IBlobStorageSettings : IOverridableSettings
    {
        IOverridableSetting<string> ContainerName { get; set; }
    }
}
