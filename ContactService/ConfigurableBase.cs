using Microsoft.Extensions.Configuration;

namespace ContactService
{
	public abstract class ConfigurableBase<T>
		where T : IConfigurable, new()
	{
		public void FromConfigurationRoot(IConfigurationRoot configurationRoot)
		{
			var sectionName = this.GetType().Name;
			var configurationSection = configurationRoot.GetSection(sectionName);
			FromConfigurationSection(configurationSection);
		}

		public void FromConfigurationSection(IConfigurationSection configurationSection)
		{
			configurationSection.Bind(this);
		}

		public static T Deserialize(IConfigurationRoot configurationRoot)
		{
			var t = new T();
			(t as ConfigurableBase<T>).FromConfigurationRoot(configurationRoot);
			return t;
		}

		public static T Deserialize(IConfigurationSection configurationSection)
		{
			var t = new T();
			(t as ConfigurableBase<T>).FromConfigurationSection(configurationSection);
			return t;
		}
	}
}