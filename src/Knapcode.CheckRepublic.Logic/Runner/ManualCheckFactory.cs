using System.Collections.Generic;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Runner.Checks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

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
            yield return new ConcertoUpCheck(GetHttpSubstringCheck());
            yield return new ConnectorRideLatestJsonCheck(GetHttpJTokenCheck());
            yield return new NuGetToolsUpCheck(GetHttpSubstringCheck());
            yield return new PoGoNotificationsHeartbeatCheck(GetHeartbeatCheck());
            yield return new UserAgentReportUpCheck(GetHttpSubstringCheck());
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
            return new HeartbeatCheck(new HeartGroupService(_context));
        }
    }
}
