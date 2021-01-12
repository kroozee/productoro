namespace Productoro.Models.Commands
{
    internal sealed record AddProjectCommand(NewProject Project) : Command;
}