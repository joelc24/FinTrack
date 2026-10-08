using FinTrack.Domain.Common;
using MediatR;

namespace FinTrack.Application.Common.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
where TQuery : IQuery<TResponse>
{
    
}