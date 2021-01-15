using System;
using System.Reactive.Subjects;
using Optional;
using Optional.Unsafe;
using Productoro.Implementation;
using Productoro.Models.Events;

namespace Productoro.Models.Aggregates
{

    public interface ITaskAggregate : IAggregate<Task>
    {
        void Handle(TaskCreated @event);
        void Handle(TaskCompleted @event);
        void Handle(TaskUncompleted @event);
    }

    internal sealed class TaskAggregate : ITaskAggregate, IObservable<(AggregateId, IDomainEvent<ITaskAggregate, Task>)>
    {
        private readonly ISubject<(AggregateId, IDomainEvent<ITaskAggregate, Task>)> subject;

        public TaskAggregate()
        {
            CurrentState = Option.None<Task>();
            subject = Subject.Synchronize(new SimpleSubject<(AggregateId, IDomainEvent<ITaskAggregate, Task>)>());
        }

        public Option<Task> CurrentState { get; private set; }

        public void Handle(TaskCreated @event)
        {
            CurrentState = Option.Some(new Task(@event.Id, @event.Name, false));
            subject.OnNext((Id, @event));
        }

        public void Handle(TaskCompleted @event)
        {
            CurrentState = CurrentState.Map(state => new Task(state.Id, state.Name, true));
            subject.OnNext((Id, @event));
        }

        public void Handle(TaskUncompleted @event)
        {
            CurrentState = CurrentState.Map(state => new Task(state.Id, state.Name, false));
            subject.OnNext((Id, @event));
        }

        public IDisposable Subscribe(IObserver<(AggregateId, IDomainEvent<ITaskAggregate, Task>)> observer) =>
            subject.Subscribe(observer);

        private AggregateId Id =>
            CurrentState
                .Map(state => new AggregateId(state.Id.Value))
                .ValueOrFailure("Aggregate has not been initialized.");
    }
}