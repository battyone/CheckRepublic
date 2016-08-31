using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Website
{
    public class WebsiteOptions
    {
        public string ReadPassword { get; set; }
        public string WritePassword { get; set; }
        public GroupMeOptions GroupMe { get; set; }
    }
}
