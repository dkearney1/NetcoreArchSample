using RabbitMQ.Client;

namespace ContactAPI
{
	public interface IConnectionFactory
	{
		IConnection Connection { get; }
	}
}