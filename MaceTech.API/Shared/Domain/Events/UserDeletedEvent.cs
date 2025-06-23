using MediatR;

namespace MaceTech.API.Shared.Domain.Events;

public record UserDeletedEvent(string Uid) : INotification;