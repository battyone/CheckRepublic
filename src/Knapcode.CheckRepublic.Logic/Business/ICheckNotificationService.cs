﻿using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface ICheckNotificationService
    {
        Task<CheckNotification> CheckForNotificationAsync(string checkName, CancellationToken token);
    }
}