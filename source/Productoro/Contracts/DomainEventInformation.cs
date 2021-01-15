using System;
using System.Runtime.Serialization;

namespace Productoro.Contracts
{
    [DataContract(Namespace = Resources.ContractNamespace, Name = "DomainEventInformation")]
    public sealed class DomainEventInformation : IExtensibleDataObject
    {
        [DataMember(Name = "Id", Order = 0)]
        public Guid Id { get; set; }

        [DataMember(Name = "AggregateType", Order = 1)]
        public string AggregateType { get; set; } = null!;

        [DataMember(Name = "AggregateId", Order = 2)]
        public Guid AggregateId { get; set; }

        [DataMember(Name = "Type", Order = 3)]
        public string Type { get; set; } = null!;

        [DataMember(Name = "Body", Order = 4)]
        public string Body { get; set; } = null!;

        [DataMember(Name = "Timestamp", Order = 5)]
        public DateTimeOffset Timestamp { get; set; }

        ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }
    }
}