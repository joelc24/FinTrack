

using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Profiles.Dtos;

namespace FinTrack.Application.Profiles.Queries.GetProfilesByAccount;

public sealed record GetProfilesByAccountQuery : IQuery<IReadOnlyList<ProfileDto>>;