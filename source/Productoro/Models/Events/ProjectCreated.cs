namespace Productoro.Models.Events
{
    internal sealed record ProjectCreated(NewProject Project);

    /*internal sealed record ProjectCreated : DomainEvent, IDomainEvent
    {
        public ProjectCreated(
            NewProject project,
            DomainEventId Id,
            Aggregate Aggregate,
            InstanceId InstanceId,
            Timestamp Timestamp)
            : base(Id, Aggregate, InstanceId, Timestamp)
        {
            Project = project;
        }

        public NewProject Project;

        public string AcceptAsync(IDomainEventVisitor visitor) =>
            visitor.VisitAsync(this);
    }*/
}