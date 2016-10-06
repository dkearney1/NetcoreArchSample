namespace ContactAPI
{
	public sealed class RabbitConfig : ConfigurableBase<RabbitConfig>, IConfigurable
	{
		public string Hostname { get; set; }
		public int Port { get; set; }
		public string VirtualHost { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool PublisherConfirms { get; set; }
    }
}