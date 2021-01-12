using System.Collections.Generic;

namespace Productoro.Models
{
    public sealed record Project(ProjectId Id, ProjectName Name, IReadOnlyCollection<Task> Tasks);
}