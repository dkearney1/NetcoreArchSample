using System;
using System.Reflection;
using RabbitMQ.Client;

namespace ContactService
{
    public sealed class RabbitConnectionFactory : IConnectionFactory, IDisposable
    {
        private bool _disposedValue;
        private readonly IConnection _connection;

        public RabbitConnectionFactory(RabbitConfig rabbitConfig)
        {
            var cf = new ConnectionFactory()
            {
                HostName = rabbitConfig.Hostname,
                Port = rabbitConfig.Port,
                VirtualHost = rabbitConfig.VirtualHost,
                UserName = rabbitConfig.Username,
                Password = rabbitConfig.Password
            };

            cf.AutomaticRecoveryEnabled = true;

            var process = Assembly.GetEntryAssembly().GetName().Name;

            _connection = cf.CreateConnection(process);
        }

        public IConnection Connection => _connection;

        #region IDisposable Support
        void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        if (_connection.IsOpen)
                            _connection.Close();

                        _connection.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RabbitConnectionFactory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}