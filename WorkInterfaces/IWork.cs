using System;

namespace WorkInterfaces
{
	public interface IWork
	{
		Guid Id { get; }
		Guid? CorrelationId { get; }
		DateTimeOffset Created { get; }
	}
}