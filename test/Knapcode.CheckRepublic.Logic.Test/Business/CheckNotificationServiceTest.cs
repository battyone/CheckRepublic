using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Knapcode.CheckRepublic.Logic.Test
{
    public class CheckNotificationServiceTest
    {
        [Fact]
        public async Task CheckForNotificationsAsync_ReturnsNullWithNoCheckResults() 
        {
            // Arrange
            using (var tc = new TestContext())
            {
                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.Null(notification);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_ReturnsNullWithNoNotificationAndIsCurrentlySuccess()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                tc.AddCheckResult(CheckResultType.Success, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.Null(notification);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_ReturnsNullWithNoNotificationAndNotEnoughFailures()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                tc.AddCheckResult(CheckResultType.Failure, -1);
                tc.AddCheckResult(CheckResultType.Failure, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.Null(notification);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_AddsNotificationWithNoNotificationAndEnoughFailures()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                var oldest = tc.AddCheckResult(CheckResultType.Failure, -2);
                tc.AddCheckResult(CheckResultType.Failure, -1);
                tc.AddCheckResult(CheckResultType.Failure, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.NotNull(notification);
                Assert.False(notification.IsHealthy);
                Assert.Equal(oldest.CheckResultId, notification.CheckResultId);
                Assert.Equal(tc.UtcNow, notification.Time);
                Assert.Equal(1, notification.Version);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_ReturnsNullWithFailuresThatAreTooOld()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                tc.AddCheckResult(CheckResultType.Failure, -16);
                tc.AddCheckResult(CheckResultType.Failure, -15);
                tc.AddCheckResult(CheckResultType.Failure, -14); // This one should be found by the query.

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.Null(notification);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_ReturnsNullWithUnhealthyNotificationAndEnoughFailures()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                tc.AddCheckNotification(false);
                tc.AddCheckResult(CheckResultType.Failure, -2);
                tc.AddCheckResult(CheckResultType.Failure, -1);
                tc.AddCheckResult(CheckResultType.Failure, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.Null(notification);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_ReturnsNullWithHealthyNotificationAndIsCurrentlySuccess()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                tc.AddCheckNotification(true);
                tc.AddCheckResult(CheckResultType.Success, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.Null(notification);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_ReturnsNullWithHealthyNotificationAndNotEnoughFailures()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                tc.AddCheckNotification(true);
                tc.AddCheckResult(CheckResultType.Failure, -1);
                tc.AddCheckResult(CheckResultType.Failure, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.Null(notification);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_UpdatesNotificationWithHealthNotificationAndEnoughFailures()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                var existing = tc.AddCheckNotification(true);
                var oldest = tc.AddCheckResult(CheckResultType.Failure, -2);
                tc.AddCheckResult(CheckResultType.Failure, -1);
                tc.AddCheckResult(CheckResultType.Failure, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.NotNull(notification);
                Assert.False(notification.IsHealthy);
                Assert.Equal(oldest.CheckResultId, notification.CheckResultId);
                Assert.Equal(tc.UtcNow, notification.Time);
                Assert.Equal(2, notification.Version);
                Assert.Equal(existing.CheckNotificationId, notification.CheckNotificationId);
            }
        }

        [Fact]
        public async Task CheckForNotificationsAsync_UpdatesNotificationWithUnhealthNotificationAndIsCurrentlySuccess()
        {
            // Arrange
            using (var tc = new TestContext())
            {
                var existing = tc.AddCheckNotification(false);
                var success = tc.AddCheckResult(CheckResultType.Success, 0);

                // Act
                var notification = await tc.Target.CheckForNotificationAsync(tc.CheckName, tc.Token);

                // Assert
                Assert.NotNull(notification);
                Assert.True(notification.IsHealthy);
                Assert.Equal(success.CheckResultId, notification.CheckResultId);
                Assert.Equal(tc.UtcNow, notification.Time);
                Assert.Equal(2, notification.Version);
                Assert.Equal(existing.CheckNotificationId, notification.CheckNotificationId);
            }
        }

        private class TestContext : IDisposable
        {
            public TestContext()
            {
                CheckName = "CheckName";
                Options = GetOptions();
                Token = CancellationToken.None;
                SystemTime = new Mock<ISystemClock>();
                UtcNow = new DateTimeOffset(2016, 1, 2, 3, 4, 5, TimeSpan.Zero);
                SystemTime.Setup(x => x.UtcNow).Returns(() => UtcNow);
                CheckContext = new CheckContext(Options);

                Target = new CheckNotificationService(SystemTime.Object, CheckContext, new EntityMapper());
            }

            public string CheckName { get; }
            public DbContextOptions<CheckContext> Options { get; }
            public CheckContext CheckContext { get; }
            public CancellationToken Token { get; }
            public Mock<ISystemClock> SystemTime { get; }
            public DateTimeOffset UtcNow { get; }
            public CheckNotificationService Target { get; }

            public CheckResult AddCheckResult(CheckResult checkResult)
            {
                using (var checkContext = new CheckContext(Options))
                {
                    checkContext.CheckResults.Add(checkResult);
                    checkContext.SaveChanges();

                    return checkResult;
                }
            }

            public CheckNotification AddCheckNotification(bool isHealthy)
            {
                using (var checkContext = new CheckContext(Options))
                {
                    var check = new Check { Name = CheckName };
                    var checkNotification = new CheckNotification
                    {
                        Check = check,
                        CheckResult = new CheckResult
                        {
                            Check = check,
                            CheckBatch = new CheckBatch(),
                            Type = isHealthy ? CheckResultType.Success : CheckResultType.Failure
                        },
                        IsHealthy = isHealthy,
                        Version = 1
                    };

                    checkContext.CheckNotifications.Add(checkNotification);
                    checkContext.SaveChanges();

                    return checkNotification;
                }
            }

            public CheckResult AddCheckResult(CheckResultType type, int minutesDelta)
            {
                return AddCheckResult(GetCheckResult(minutesDelta, type));
            }

            private CheckResult GetCheckResult(int minutesDelta, CheckResultType type)
            {
                return new CheckResult
                {
                    Check = new Check
                    {
                        Name = CheckName
                    },
                    CheckBatch = new CheckBatch(),
                    Type = type,
                    Time = TimeUtilities.DateTimeOffsetToLong(UtcNow.AddMinutes(minutesDelta))
                };
            }

            private DbContextOptions<CheckContext> GetOptions()
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                var builder = new DbContextOptionsBuilder<CheckContext>();

                builder
                    .UseInMemoryDatabase(GetType().FullName)
                    .UseInternalServiceProvider(serviceProvider);

                return builder.Options;
            }

            public void Dispose()
            {
                CheckContext.Dispose();
            }
        }
    }
}
