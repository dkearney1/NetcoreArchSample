using System;
using System.Text;
using System.Threading.Tasks;
using ContactEntity;
using ContactWorkImplementations;
using WorkInterfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace ContactAPI.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IModel _channel;
        private readonly Encoding _encoding;
        private readonly JsonSerializerSettings _serializationSettings;

        public ContactController(IConnectionFactory connectionFactory, Encoding encoding)
        {
            _connectionFactory = connectionFactory;
            _channel = _connectionFactory.Connection.CreateModel();
            _encoding = encoding;
            _serializationSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
        }

        [HttpGet]
        public IActionResult GetCount()
        {
            var request = new ContactGetCount();
            using (var rpcClient = new ContactRpcClient<ContactGetCount, ContactGetCountResponse>(_connectionFactory, _encoding))
            {
                var response = rpcClient.RemoteCall(request);
                return Ok(response.Count);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var request = new ContactGet(id);
            using (var rpcClient = new ContactRpcClient<ContactGet, ContactGetResponse>(_connectionFactory, _encoding))
            {
                var response = rpcClient.RemoteCall(request);
                return Ok(response.Contact);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Contact contact)
        {
            var request = new ContactCreate(contact.Name);
            using (var rpcClient = new ContactRpcClient<ContactCreate, ContactCreateResponse>(_connectionFactory, _encoding))
            {
                var response = rpcClient.RemoteCall(request);
                return Ok(response.Contact);
            }
        }
    }
}