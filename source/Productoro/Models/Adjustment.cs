using System;

namespace Productoro.Models
{
    public record Adjustment(TaskId TaskId, TimeSpan Value);
}