using System.Collections.Generic;

namespace Productoro.Models.Commands
{
    internal sealed record ArchiveProjectCommand(ProjectId ProjectId) : Command;
}