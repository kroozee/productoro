using Optional;

namespace Productoro.Models.Aggregates
{
    public interface IAggregate<TState>
    {
        Option<TState> CurrentState { get; }
    }
}