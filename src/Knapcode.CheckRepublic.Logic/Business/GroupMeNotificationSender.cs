using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.GroupMe;
using Knapcode.GroupMe.Models;
using Microsoft.Extensions.Options;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class GroupMeNotificationSender : INotificationSender
    {
        private readonly IOptions<GroupMeOptions> _options;
        private readonly BotService _botService;

        public GroupMeNotificationSender(IOptions<GroupMeOptions> options)
        {
            _options = options;
            _botService = new BotService(_options.Value.AccessToken, new HttpClient());
        }

        public async Task SendNotificationAsync(string text, CancellationToken token)
        {
            var message = new BotMessage { Text = text };

            await _botService.PostAsync(_options.Value.BotId, message, token);
        }
    }
}
