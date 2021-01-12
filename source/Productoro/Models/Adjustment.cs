using System;
using System.Collections.Generic;

namespace Productoro.Models
{
    public record Adjustment(TaskId TaskId, TimeSpan Value);
}