using Microsoft.Extensions.Configuration;

namespace ContactService
{
	public interface IConfigurable
	{
		void FromConfigurationRoot(IConfigurationRoot configurationRoot);
		void FromConfigurationSection(IConfigurationSection configurationSection);
	}
}