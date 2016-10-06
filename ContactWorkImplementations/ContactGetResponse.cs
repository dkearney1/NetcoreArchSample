using System;
using ContactEntity;
using WorkInterfaces;

namespace ContactWorkImplementations
{
    public sealed class ContactGetResponse : IWorkResponse
    {
        public Guid Id { get; set; }
        public Guid? CorrelationId { get; set; }
        public DateTimeOffset Created { get; set; }
        public Contact Contact { get; set; }

        public ContactGetResponse()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
        }

		public ContactGetResponse(Contact contact)
			: this()
		{
            Contact = contact;
		}

    }
}