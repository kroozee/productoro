using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Productoro.Models;

namespace Productoro
{
    public interface IProjectClient
    {
        ValueTask<IReadOnlyCollection<Project>> GetAsync();
        ValueTask<Option<Project>> GetAsync(ProjectId id);
    }
}