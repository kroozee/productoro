using System;

namespace Productoro.Models.Commands
{
    internal sealed record CommandId(Guid Value);

    internal record Command
    {
        public Command() => CommandId = new CommandId(Guid.NewGuid());
        public CommandId CommandId { get; }
    }
}