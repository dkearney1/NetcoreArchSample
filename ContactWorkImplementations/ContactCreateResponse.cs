using System;
using ContactEntity;
using WorkInterfaces;

namespace ContactWorkImplementations
{
    public sealed class ContactCreateResponse : IWorkResponse
    {
        public Guid Id { get; set; }
        public Guid? CorrelationId { get; set; }
        public DateTimeOffset Created { get; set; }
        public Contact Contact { get; set; }

        public ContactCreateResponse()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
        }

        public ContactCreateResponse(Contact contact)
            : this()
        {
            Contact = contact;
        }

    }
}