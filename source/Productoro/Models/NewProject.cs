using System.Collections.Generic;

namespace Productoro.Models
{
    public record NewProject(ProjectName Name, IReadOnlyCollection<NewTask> Tasks);
}