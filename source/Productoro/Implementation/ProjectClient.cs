using System;
using System.Threading.Tasks;
using Optional;
using Productoro.Models;
using Productoro.Models.Aggregates;
using Productoro.Models.Events;

namespace Productoro.Implementation
{

    internal sealed class ProjectClient : BaseClient<IProjectAggregate, Project>, IProjectClient
    {
        public ProjectClient(IEventStore store)
            : base(store)
        {
        }

        protected override AggregateType AggregateType { get; } =
            AggregateTypes.Project;

        protected override Func<IProjectAggregate> AggregateFactory { get; } =
            () => new ProjectAggregate();

        public ValueTask<Option<Project>> GetAsync(ProjectId id) =>
            GetAsync(new AggregateId(id.Value));
    }
}