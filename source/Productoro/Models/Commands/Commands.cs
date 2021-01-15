using System;

namespace Productoro.Models.Commands
{
    internal sealed record CommandId(Guid Value);

    internal record Command
    {
        public Command() => CommandId = new CommandId(Guid.NewGuid());
        public CommandId CommandId { get; }
    }

    internal sealed record AddProjectCommand(NewProject Project) : Command;

    internal sealed record AddSessionCommand(Session Value) : Command;

    internal sealed record ArchiveProjectCommand(ProjectId ProjectId) : Command;

    internal sealed record CompleteTaskCommand(ProjectId ProjectId, TaskId TaskId) : Command;

    internal sealed record MakeAdjustmentCommand(Adjustment Adjustment) : Command;

    internal sealed record UncompleteTaskCommand(ProjectId ProjectId, TaskId TaskId) : Command;
}