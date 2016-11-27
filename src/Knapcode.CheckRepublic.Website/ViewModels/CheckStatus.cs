using System.Collections.Generic;
using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Website.ViewModels
{
    public class CheckStatus
    {
        public string CheckName { get; set; }
        public IList<CheckResult> Items { get; set; }
    }
}
