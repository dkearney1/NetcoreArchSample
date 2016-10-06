using System;

namespace WorkInterfaces
{
	public interface IWorkResponse
	{
		Guid Id { get; }
		Guid? CorrelationId { get; }
		DateTimeOffset Created { get; }
	}
}