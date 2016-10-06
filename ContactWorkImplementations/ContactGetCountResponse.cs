using System;
using WorkInterfaces;

namespace ContactWorkImplementations
{
    public sealed class ContactGetCountResponse : IWorkResponse
    {
        public Guid Id { get; set; }
        public Guid? CorrelationId { get; set; }
        public DateTimeOffset Created { get; set; }
		public int Count { get; set; }

        public ContactGetCountResponse()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
        }

		public ContactGetCountResponse(int count)
			: this()
		{
			Count = count;
		}
    }
}