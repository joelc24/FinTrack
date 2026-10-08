using FinTrack.Domain.Common;
using MediatR;

namespace FinTrack.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}