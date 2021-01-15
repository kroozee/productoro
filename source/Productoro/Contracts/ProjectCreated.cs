using System;
using System.Runtime.Serialization;

namespace Productoro.Contracts
{
    [DataContract(Namespace = Resources.ContractNamespace, Name = "ProjectCreated")]
    public sealed class ProjectCreated : IExtensibleDataObject
    {
        [DataMember(Name = "Id", Order = 0)]
        public Guid Id { get; set; }

        [DataMember(Name = "Name", Order = 1)]
        public string Name { get; set; } = null!;
        ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }
    }
}