using System.Collections.Generic;
using Knapcode.CheckRepublic.Logic.Runner.Checks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public class ManualCheckFactory : ICheckFactory
    {
        public IEnumerable<ICheck> BuildAll()
        {
            yield return new BlogUpCheck(new HttpCheck());
            yield return new ConcertoUpCheck(new HttpCheck());
            yield return new NuGetToolsUpCheck(new HttpCheck());
            yield return new UserAgentReportUpCheck(new HttpCheck());
            yield return new WintalloUpCheck(new HttpCheck());
        }
    }
}
