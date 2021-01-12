using System;

namespace Productoro.Extensions
{
    internal static class ContractExtensions
    {
        public static Models.Events.DomainEvent ToModel(this Contracts.DomainEvent @this, object body)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (body is null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            // TODO: more validation?
            return new Models.Events.DomainEvent(
                new Models.Events.DomainEventId(@this.Id),
                new Aggregate(@this.Aggregate),
                new Models.Events.InstanceId(@this.InstanceId),
                body,
                new Models.Events.Timestamp(@this.Timestamp));
        }
    }
}