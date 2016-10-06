using RabbitMQ.Client;

namespace ContactService
{
	public interface IConnectionFactory
	{
		IConnection Connection { get; }
	}
}