namespace Productoro.Models.Commands
{
    internal sealed record UncompleteTaskCommand(ProjectId ProjectId, TaskId TaskId) : Command;
}