using System;

namespace Productoro.Models.Events
{
    /*internal interface IDomainEvent
    {
        DomainEventId Id { get; }
        Aggregate Aggregate { get; }
        InstanceId InstanceId { get; }
        Timestamp Timestamp { get; }
        string AcceptAsync(IDomainEventVisitor visitor);
    }

    internal interface IDomainEventVisitor
    {
        string VisitAsync(ProjectCreated @event);
    }*/

    internal sealed record DomainEventId(Guid Value);
    internal sealed record InstanceId(Guid Value);
    internal sealed record Timestamp(DateTimeOffset Value);
    internal sealed record DomainEvent(DomainEventId Id, Aggregate Aggregate, InstanceId InstanceId, object Body, Timestamp Timestamp);
}