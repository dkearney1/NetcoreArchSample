using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using WorkInterfaces;

namespace ContactAPI
{
    internal sealed class ContactRpcClient<W, R> : SimpleRpcClient
        where W : IWork
        where R : IWorkResponse
    {
        private readonly Encoding _encoding;

        public ContactRpcClient(IConnectionFactory connectionFactory, Encoding encoding)
            : base(connectionFactory.Connection.CreateModel(), string.Format("ContactService_{0}", typeof(W).Name))
        {
            _encoding = encoding;
        }

        public R RemoteCall(W request, int timeoutMilliseconds)
        {
            this.TimeoutMilliseconds = timeoutMilliseconds;
            return this.RemoteCall(request);
        }

        public R RemoteCall(W request)
        {
            var requestStr = JsonConvert.SerializeObject(
                request,
                Formatting.None,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            var requestBytes = _encoding.GetBytes(requestStr);

            var requestProps = this.Model.CreateBasicProperties();
            requestProps.DeliveryMode = 1; //non-persistent
            requestProps.ContentEncoding = "utf-8";
            requestProps.ContentType = "application/json";
            requestProps.CorrelationId = request.CorrelationId.ToString();
            requestProps.Type = request.GetType().FullName;

            IBasicProperties responseProps;
            var responseBytes = this.Call(requestProps, requestBytes, out responseProps);

            var responseStr = _encoding.GetString(responseBytes);
            var responseObj = JsonConvert.DeserializeObject<R>(responseStr);

            return responseObj;
        }
    }
}