// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Concurrent;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProactiveDemo
{
    public class EmptyBot : ActivityHandler
    {
        private ConcurrentDictionary<string, ConversationReference> _userConversationReferences;

        public EmptyBot(ConcurrentDictionary<string, ConversationReference> userConversationReferences)
        {
            _userConversationReferences = userConversationReferences;
        }
        
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello world!"), cancellationToken);
                }
            }
        }

        protected override Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {

            if (turnContext.Activity is Activity activity)
            {
                var conReference = activity.GetConversationReference();

                _userConversationReferences.AddOrUpdate(conReference.User.Id, conReference,
                    (key, newValue) => conReference);
            }
            
            return base.OnConversationUpdateActivityAsync(turnContext, cancellationToken);
        }
    }
}
