using System;
using Knapcode.CheckRepublic.Logic.Utilities;
using Xunit;

namespace Knapcode.CheckRepublic.Logic.Test.Utilities
{
    public class TimeUtilitiesTest
    {
        [Fact]
        public void DateTimeOffset_WithUtcNow_RoundTrips()
        {
            // Arrange
            var expected = DateTimeOffset.UtcNow;

            // Act
            var intermediate = TimeUtilities.DateTimeOffsetToLong(expected);
            var actual = TimeUtilities.LongToDateTimeOffset(intermediate);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DateTimeOffset_WithNow_RoundTrips()
        {
            // Arrange
            var expected = DateTimeOffset.Now;

            // Act
            var intermediate = TimeUtilities.DateTimeOffsetToLong(expected);
            var actual = TimeUtilities.LongToDateTimeOffset(intermediate);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DateTimeOffset_WithArbitrary_RoundTrips()
        {
            // Arrange
            var expected = new DateTimeOffset(2016, 1, 2, 3, 4, 5, 6, TimeSpan.Zero);

            // Act
            var intermediate = TimeUtilities.DateTimeOffsetToLong(expected);
            var actual = TimeUtilities.LongToDateTimeOffset(intermediate);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeSpan_RoundTrips()
        {
            // Arrange
            var expected = new TimeSpan(1, 2, 3, 4, 5);

            // Act
            var intermediate = TimeUtilities.TimeSpanToLong(expected);
            var actual = TimeUtilities.LongToTimeSpan(intermediate);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
