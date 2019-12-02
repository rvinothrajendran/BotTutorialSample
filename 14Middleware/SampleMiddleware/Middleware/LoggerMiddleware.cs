using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleMiddleware.Middleware
{
    public class LoggerMiddleware : IMiddleware
    {
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
        {
            //logger 
            await turnContext.SendActivityAsync("Hey am Logger Middleware");
            await next(cancellationToken);
        }
    }

    public class GrammerMiddleware : IMiddleware
    {
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
        {
            await turnContext.SendActivityAsync("Hey am Grammer Middleware");
            await next(cancellationToken);
        }
    }
}
