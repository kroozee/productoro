using Productoro.Models.Events;

namespace Productoro
{
    internal static class Aggregates
    {
        public static Aggregate Project = new Aggregate("Project");
        public static Aggregate Task = new Aggregate("Task");
    }
}