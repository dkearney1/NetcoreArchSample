using System;
using WorkInterfaces;

namespace ContactWorkImplementations
{
    public sealed class ContactGetCount : IWork
    {
        public Guid Id { get; set; }
        public Guid? CorrelationId { get; set; }
        public DateTimeOffset Created { get; set; }

        public ContactGetCount()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
        }
    }
}