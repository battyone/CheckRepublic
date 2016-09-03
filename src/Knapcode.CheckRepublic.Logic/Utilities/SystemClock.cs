using System;

namespace Knapcode.CheckRepublic.Logic.Utilities
{
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
