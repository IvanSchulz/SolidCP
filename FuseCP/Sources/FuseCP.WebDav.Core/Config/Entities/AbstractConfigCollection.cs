using System.Configuration;
using FuseCP.WebDavPortal.WebConfigSections;

namespace FuseCP.WebDav.Core.Config.Entities
{
    public abstract class AbstractConfigCollection
    {
        protected WebDavExplorerConfigurationSettingsSection ConfigSection;

        protected AbstractConfigCollection()
        {
            ConfigSection =
                (WebDavExplorerConfigurationSettingsSection)
                    ConfigurationManager.GetSection(WebDavExplorerConfigurationSettingsSection.SectionName);
        }
    }
}
