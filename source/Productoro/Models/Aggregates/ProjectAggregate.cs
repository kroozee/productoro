using System;
using System.Collections.Immutable;
using System.Reactive.Subjects;
using Optional;
using Optional.Unsafe;
using Productoro.Implementation;
using Productoro.Models.Events;

namespace Productoro.Models.Aggregates
{
    public interface IProjectAggregate : IAggregate<Project>
    {
        void Handle(ProjectCreated @event);
    }

    internal sealed class ProjectAggregate : IProjectAggregate, IObservable<(AggregateId, IDomainEvent<IProjectAggregate, Project>)>
    {
        private readonly ISubject<(AggregateId, IDomainEvent<IProjectAggregate, Project>)> subject;

        public ProjectAggregate()
        {
            CurrentState = Option.None<Project>();
            subject = Subject.Synchronize(new SimpleSubject<(AggregateId, IDomainEvent<IProjectAggregate, Project>)>());
        }

        public Option<Project> CurrentState { get; private set; }

        public void Handle(ProjectCreated @event)
        {
            CurrentState = Option.Some(new Project(@event.Id, @event.Name, ImmutableArray<Task>.Empty, false));
            subject.OnNext((Id, @event));
        }

        public IDisposable Subscribe(IObserver<(AggregateId, IDomainEvent<IProjectAggregate, Project>)> observer) =>
            subject.Subscribe(observer);

        private AggregateId Id =>
            CurrentState
                .Map(state => new AggregateId(state.Id.Value))
                .ValueOrFailure("Aggregate has not been initialized.");
    }
}