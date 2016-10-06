using System;
using WorkInterfaces;

namespace ContactWorkImplementations
{
    public sealed class ContactGet : IWork
    {
        public Guid Id { get; set; }
        public Guid? CorrelationId { get; set; }
        public DateTimeOffset Created { get; set; }
		public Guid ContactId { get; set; }

        public ContactGet()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
        }

		public ContactGet(Guid contactId)
			: this()
		{
			ContactId = contactId;
		}
    }
}