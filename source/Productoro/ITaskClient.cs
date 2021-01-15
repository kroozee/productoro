using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Productoro.Models;

namespace Productoro
{
    public interface ITaskClient
    {
        ValueTask<IReadOnlyCollection<Models.Task>> GetAsync();
        ValueTask<Option<Models.Task>> GetAsync(TaskId id);
    }
}