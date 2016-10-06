using System;
using ContactEntity;

namespace ContactService
{
	public interface IContactRepository
	{
        int GetCount();
        Contact GetContact(Guid id);
        Contact CreateContact(Guid id, string name);
    }
}