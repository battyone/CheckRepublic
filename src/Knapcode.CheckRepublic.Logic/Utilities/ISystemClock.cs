using System;

namespace Knapcode.CheckRepublic.Logic.Utilities
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}