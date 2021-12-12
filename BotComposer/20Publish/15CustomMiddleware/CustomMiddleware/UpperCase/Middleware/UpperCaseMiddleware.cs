using Microsoft.Bot.Builder;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace BotComposerMiddlewareComponent.Middleware
{
    public class UpperCaseMiddleware : IMiddleware
    {
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (turnContext?.Activity.Type == ActivityTypes.Message
            && !string.IsNullOrEmpty(turnContext.Activity.Text))
            {
                turnContext.Activity.Text = turnContext.Activity.Text.ToUpper();
            }

            await next(cancellationToken);

        }
    }
}
