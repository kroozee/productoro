using System;
using Newtonsoft.Json;
using Productoro.Models.Events;

namespace Productoro.Extensions
{
    internal static class ModelExtensions
    {
        public static Contracts.DomainEventInformation ToContract(this ProjectCreated @this)
        {
            if (@this is null)
            {
                throw new System.ArgumentNullException(nameof(@this));
            }

            var eventContract = new Contracts.ProjectCreated()
            {
                Id = @this.Id.Value,
                Name = @this.Name.Value
            };

            return new Contracts.DomainEventInformation()
            {
                Id = Guid.NewGuid(),
                AggregateType = AggregateTypes.Project.Value,
                AggregateId = @this.Id.Value,
                Body = JsonConvert.SerializeObject(eventContract, Formatting.Indented), // TODO: extract dependencies.
                Timestamp = DateTimeOffset.Now
            };
        }
    }
}