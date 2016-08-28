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
            yield return new BlogUpCheck(GetHttpCheck());
            yield return new ConcertoUpCheck(GetHttpCheck());
            yield return new NuGetToolsUpCheck(GetHttpCheck());
            yield return new PoGoNotificationsHeartbeatCheck(GetHeartbeatCheck());
            yield return new UserAgentReportUpCheck(GetHttpCheck());
            yield return new WintalloUpCheck(GetHttpCheck());
        }

        private HttpCheck GetHttpCheck()
        {
            return new HttpCheck();
        }

        private HeartbeatCheck GetHeartbeatCheck()
        {
            return new HeartbeatCheck(new HeartGroupService(_context));
        }
    }
}
