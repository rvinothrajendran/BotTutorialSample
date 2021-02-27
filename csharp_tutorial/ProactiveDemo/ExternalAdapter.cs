using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;


namespace ProactiveDemo
{
    [Route("api/notify")]
    [ApiController]
    public class ExternalAdapter : ControllerBase
    {
        private IBotFrameworkHttpAdapter _externAdapter;
        private ConcurrentDictionary<string, ConversationReference> _userReference;
        public ExternalAdapter(IBotFrameworkHttpAdapter adapter, ConcurrentDictionary<string, ConversationReference> conReferences)
        {
            _externAdapter = adapter;
            _userReference = conReferences;
        }

        public async Task<IActionResult> Get()
        {
            foreach (var conversationReference in _userReference.Values)
            {
                await ((BotAdapter) _externAdapter).ContinueConversationAsync(string.Empty, conversationReference,
                    ExternalCallback, default(CancellationToken));
            }


            var result = new ContentResult();
            result.StatusCode = (int) HttpStatusCode.OK;
            result.ContentType = "text/html";
            result.Content = "<html> Hey I sent the message to the users </html>";
            
            return result;

        }

        private async Task ExternalCallback(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(MessageFactory.Text("Hey am external source"), cancellationToken);
        }
    }
}
