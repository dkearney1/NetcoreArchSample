using System;
using WorkInterfaces;

namespace ContactWorkImplementations
{
    public sealed class ContactCreate : IWork
    {
        public Guid Id { get; set; }
        public Guid? CorrelationId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Name { get; set; }

        public ContactCreate()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
        }

        public ContactCreate(string name)
            : this()
        {
            Name = name;
        }
    }
}