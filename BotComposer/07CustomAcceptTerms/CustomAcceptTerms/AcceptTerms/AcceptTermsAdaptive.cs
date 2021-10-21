using System;
using System.Collections.Generic;
using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace AcceptTerms
{
    internal class AcceptTermsAdaptive
    {
        AdaptiveCard card = null;
        public AcceptTermsAdaptive()
        {
            
        }

        private void DisplayInformation(string information, string actionName)
        {
            card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 1))
            {
                Body = new List<AdaptiveElement>()
            };

            card.Body.Add(PrepareInputType(information));
            card.Actions.Add(SubmitAction(actionName));
        }

        private AdaptiveToggleInput PrepareInputType(string information)
        {
            var toggle = new AdaptiveToggleInput
            {
                Id = "Accept",
                Title = information,
                Value = "yes",
                ValueOn = "yes",
                ValueOff = "no"
            };

            return toggle;
        }

        private AdaptiveAction SubmitAction(string actionName)
        {
            AdaptiveAction actionSubmit = new AdaptiveSubmitAction
            {
                Type = "Action.Submit",
                Title = actionName                
            };

            return actionSubmit;
        }

        public Attachment CreateAttachment(string information,string actionName)
        {

            DisplayInformation(information,actionName);

            var attachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = JObject.FromObject(card)                
            };

            return attachment;
        }
        
    }
}
