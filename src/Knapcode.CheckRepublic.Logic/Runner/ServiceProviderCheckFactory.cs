using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public class ServiceProviderCheckFactory : ICheckFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderCheckFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ICheck> BuildAll()
        {
            return _serviceProvider.GetServices<ICheck>();
        }
    }
}
