namespace Productoro.Models.Commands
{
    internal sealed record CompleteTaskCommand(ProjectId ProjectId, TaskId TaskId) : Command;
}