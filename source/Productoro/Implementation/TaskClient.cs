using System;
using System.Threading.Tasks;
using Optional;
using Productoro.Models;
using Productoro.Models.Aggregates;
using Productoro.Models.Events;

namespace Productoro.Implementation
{
    internal sealed class TaskClient : BaseClient<ITaskAggregate, Models.Task>, ITaskClient
    {
        public TaskClient(IEventStore store)
            : base(store)
        {
        }

        protected override AggregateType AggregateType { get; } =
            AggregateTypes.Task;

        protected override Func<ITaskAggregate> AggregateFactory { get; } =
            () => new TaskAggregate();

        public ValueTask<Option<Models.Task>> GetAsync(TaskId id) =>
            GetAsync(new AggregateId(id.Value));
    }
}