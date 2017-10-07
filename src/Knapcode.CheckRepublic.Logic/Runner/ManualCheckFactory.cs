using System.Collections.Generic;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Runner.Checks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;
using Knapcode.CheckRepublic.Logic.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public class ManualCheckFactory : ICheckFactory
    {
        private readonly CheckContext _context;

        public ManualCheckFactory(CheckContext context)
        {
            _context = context;
        }

        public IEnumerable<ICheck> BuildAll()
        {
            yield return new BlogUpCheck(GetHttpSubstringCheck());
            yield return new ConnectorRideLatestJsonCheck(GetHttpJTokenCheck());
            yield return new ConnectorRideScrapeStatusCheck(new SystemClock(), GetHttpJTokenCheck());
            yield return new NuGetToolsUpCheck(GetHttpSubstringCheck());
            yield return new WintalloUpCheck(GetHttpSubstringCheck());
        }

        private HttpSubstringCheck GetHttpSubstringCheck()
        {
            return new HttpSubstringCheck(new HttpResponseStreamCheck());
        }

        private HttpJTokenCheck GetHttpJTokenCheck()
        {
            return new HttpJTokenCheck(new HttpResponseStreamCheck());
        }

        private HeartbeatCheck GetHeartbeatCheck()
        {
            return new HeartbeatCheck(new SystemClock(), new HeartGroupService(_context, new EntityMapper()));
        }
    }
}
