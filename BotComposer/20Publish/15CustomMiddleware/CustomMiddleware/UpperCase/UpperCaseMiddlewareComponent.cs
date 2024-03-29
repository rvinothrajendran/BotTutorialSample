﻿using BotComposerMiddlewareComponent.Middleware;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BotComposerMiddlewareComponent
{
    public class UpperCaseMiddlewareComponent : BotComponent
    {
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.AddSingleton<IMiddleware, UpperCaseMiddleware>();

        }
    }
}
