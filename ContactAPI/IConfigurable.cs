using Microsoft.Extensions.Configuration;

namespace ContactAPI
{
	public interface IConfigurable
	{
		void FromConfigurationRoot(IConfigurationRoot configurationRoot);
		void FromConfigurationSection(IConfigurationSection configurationSection);
	}
}