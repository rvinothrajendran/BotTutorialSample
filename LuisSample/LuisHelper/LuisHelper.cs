using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;

namespace LuisSample.LuisHelper
{
    public class LuisHelper : IRecognizer
    {
        private readonly LuisRecognizer _luisRecognizer;

        public LuisHelper()
        {

            var service = new LuisService()
            {
                AppId = "",
                SubscriptionKey = "",
                Region = "westus",
                Version = "2.0"
            };

            var app = new LuisApplication(service);
            var regOptions = new LuisRecognizerOptionsV2(app)
            {
                IncludeAPIResults = true,
                PredictionOptions = new LuisPredictionOptions()
                {
                    IncludeAllIntents = true,
                    IncludeInstanceData = true
                }
            };

            _luisRecognizer = new LuisRecognizer(regOptions);

        }

        public async Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            return await _luisRecognizer.RecognizeAsync(turnContext, cancellationToken);
        }

        public async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken) where T : IRecognizerConvert, new()
        {
            return await _luisRecognizer.RecognizeAsync<T>(turnContext, cancellationToken);
        }
    }
}
