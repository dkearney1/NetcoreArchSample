using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContactWorkImplementations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using WorkInterfaces;

namespace ContactService
{
    internal sealed class GetCountHandler : IDisposable
    {
        private readonly IModel _channel;
        private readonly Encoding _encoding;
        private readonly IContactRepository _contactRepository;

        private bool _disposedValue;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;
        private ContactRpcServer<ContactGetCount, ContactGetCountResponse> _rpcServer;
        private Subscription _subscription;

        public GetCountHandler(IConnectionFactory connectionFactory, Encoding encoding, IContactRepository contactRepository)
        {
            _channel = connectionFactory.Connection.CreateModel();
            _encoding = encoding;
            _contactRepository = contactRepository;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var queuename = CreateCompetingConsumerQueueName;
            _channel.ExchangeDeclare(queuename, "fanout", true, false, null);
            _channel.QueueDeclare(queuename, true, false, false, null);
            _channel.QueueBind(queuename, queuename, string.Empty, null);
            _subscription = new Subscription(_channel, queuename, false);

            _rpcServer = new ContactRpcServer<ContactGetCount, ContactGetCountResponse>(_subscription, _encoding, WorkHandler);
            _task = Task.Factory.StartNew(DoWork, _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _subscription?.Close();
            _cancellationTokenSource?.Cancel();
            _task?.Wait();
            _task = null;
            _cancellationTokenSource = null;
            _subscription = null;
        }

        private string CreateCompetingConsumerQueueName => $"{nameof(ContactService)}_{nameof(ContactGetCount)}";

        private async Task DoWork()
        {
            var cancelToken = _cancellationTokenSource.Token;

            try
            {
                _rpcServer.MainLoop();

                // while (!cancelToken.IsCancellationRequested)
                //     await Task.Delay(TimeSpan.FromMilliseconds(100d));

                _rpcServer.Close();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }

        private ContactGetCountResponse WorkHandler(ContactGetCount request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var count = _contactRepository.GetCount();
            var response = new ContactGetCountResponse(count) { CorrelationId = request.CorrelationId };

            return response;
        }

        #region IDisposable Support
        void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _subscription?.Close();
                    _subscription = null;

                    _cancellationTokenSource?.Cancel();
                    _cancellationTokenSource = null;

                    _task?.Wait();
                    _task = null;
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GetCountHandler() {
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