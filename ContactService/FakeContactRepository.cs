using System;
using System.Collections.Generic;
using ContactEntity;

namespace ContactService
{
    internal sealed class FakeContactRepository : IContactRepository
    {
        private readonly IDictionary<Guid, Contact> _contacts;

        public FakeContactRepository()
        {
            _contacts = new Dictionary<Guid, Contact>();
        }

        public int GetCount()
        {
            return _contacts.Count;
        }

        public Contact GetContact(Guid id)
        {
            if (_contacts.ContainsKey(id))
                return _contacts[id];
            return null;
        }

        public Contact CreateContact(Guid id, string name)
        {
            if (_contacts.ContainsKey(id))
                return _contacts[id];

            var newContact = new Contact()
            {
                Id = id,
                Name = name,
            };

            _contacts.Add(id, newContact);

            return newContact;
        }

    }
}