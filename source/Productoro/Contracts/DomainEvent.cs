using System;
using System.Runtime.Serialization;

namespace Productoro.Contracts
{
    [DataContract(Namespace = Resources.ContractNamespace, Name = "DomainEvent")]
    public abstract class DomainEvent : IExtensibleDataObject
    {
        [DataMember(Name = "Id", Order = 0)]
        public Guid Id { get; set; }

        [DataMember(Name = "Aggregate", Order = 1)]
        public string Aggregate { get; set; } = null!;

        [DataMember(Name = "InstanceId", Order = 2)]
        public Guid InstanceId { get; set; }

        [DataMember(Name = "Type", Order = 3)]
        public string Type { get; set; } = null!;

        [DataMember(Name = "Body", Order = 4)]
        public string Body { get; set; } = null!;

        [DataMember(Name = "Timestamp", Order = 5)]
        public DateTimeOffset Timestamp { get; set; }

        ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }
    }
}