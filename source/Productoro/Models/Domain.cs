using System;
using System.Collections.Generic;
using Productoro.Models.Events;

namespace Productoro.Models
{
    public sealed record ProjectId(Guid Value)
    {
        public static implicit operator AggregateId(ProjectId id) => new AggregateId(id.Value);
    }

    public sealed record ProjectName(string Value);

    public sealed record NewProject(ProjectName Name);

    public sealed record Project(ProjectId Id, ProjectName Name, IReadOnlyCollection<Task> Tasks, bool IsArchived);

    public sealed record TaskId(Guid Value)
    {
        public static implicit operator AggregateId(TaskId id) => new AggregateId(id.Value);
    }

    public sealed record TaskName(string Value);

    public sealed record NewTask(TaskName Name);

    public sealed record Task(TaskId Id, TaskName Name, bool IsCompleted);

    public sealed record Session(DateTimeOffset Start, TimeSpan Elapsed);

    public sealed record Adjustment(DateTimeOffset Start, TimeSpan Elapsed);

    internal sealed record DatabaseConnectionString(string Value);
}