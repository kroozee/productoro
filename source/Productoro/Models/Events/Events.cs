using System;
using Productoro.Models.Aggregates;

namespace Productoro.Models.Events
{
    public interface IDomainEvent<TAggregate, TState>
        where TAggregate : IAggregate<TState>
    {
        void Accept(TAggregate aggregate);
    }

    public sealed record ProjectCreated(ProjectId Id, ProjectName Name) : IDomainEvent<IProjectAggregate, Project>
    {
        public void Accept(IProjectAggregate aggregate) =>
            aggregate.Handle(this);
    }

    public sealed record TaskCreated(TaskId Id, TaskName Name) : IDomainEvent<ITaskAggregate, Task>
    {
        public void Accept(ITaskAggregate aggregate) =>
            aggregate.Handle(this);
    }

    public sealed record TaskCompleted() : IDomainEvent<ITaskAggregate, Task>
    {
        public void Accept(ITaskAggregate aggregate) =>
            aggregate.Handle(this);
    }


    public sealed record TaskUncompleted() : IDomainEvent<ITaskAggregate, Task>
    {
        public void Accept(ITaskAggregate aggregate) =>
            aggregate.Handle(this);
    }

    public sealed record DomainEventId(Guid Value);
    public sealed record AggregateType(string Value);
    public sealed record AggregateId(Guid Value);
    public sealed record Timestamp(DateTimeOffset Value);
    public sealed record DomainEventInformation<TAggregate, TState>(DomainEventId Id, AggregateId AggregateId, IDomainEvent<TAggregate, TState> Event, Timestamp Timestamp)
        where TAggregate : IAggregate<TState>;
}