using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using WorkInterfaces;

namespace ContactService
{
    internal sealed class ContactRpcServer<W, R> : SimpleRpcServer
        where W : IWork
        where R : IWorkResponse
    {
        private readonly Encoding _encoding;
        private readonly Func<W, R> _workHandler;

        public ContactRpcServer(Subscription subscription, Encoding encoding, Func<W, R> workHandler)
            : base(subscription)
        {
            _encoding = encoding;
            _workHandler = workHandler;
        }

        public override byte[] HandleSimpleCall(
            bool isRedelivered,
            IBasicProperties requestProperties,
            byte[] body,
            out IBasicProperties replyProperties)
        {
            replyProperties = requestProperties;
            replyProperties.MessageId = Guid.NewGuid().ToString();

            var requestString = _encoding.GetString(body);
            var requestObject = JsonConvert.DeserializeObject<W>(requestString);
            var responseObject = _workHandler(requestObject);
            var responseString = JsonConvert.SerializeObject(
				responseObject,
				Formatting.None,
				new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            return _encoding.GetBytes(responseString);
        }
    }
}