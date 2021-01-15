using Productoro.Models.Events;

namespace Productoro
{
    internal static class AggregateTypes
    {
        public static AggregateType Project = new AggregateType("Project");
        public static AggregateType Task = new AggregateType("Task");
    }
}