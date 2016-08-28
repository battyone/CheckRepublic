using System;

namespace Knapcode.CheckRepublic.Logic.Utilities
{
    public static class ValidationUtility
    {
        public static void ValidatePagingParameters(int skip, int take)
        {
            if (skip < 0)
            {
                throw new ArgumentOutOfRangeException("The skip value cannot be negative.", nameof(skip));
            }

            if (take < 0)
            {
                throw new ArgumentOutOfRangeException("The take value cannot be negative.", nameof(skip));
            }
        }
    }
}
