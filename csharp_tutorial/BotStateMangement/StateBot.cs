// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace StateBot
{
    public enum ConTrack
    {
        Name,
        LastName,
        Complete
    }
    public class ConvState
    {
        public ConTrack conTrack = ConTrack.Name;
    }

    public class Userprofile
    {
        public string username { get; set; }
        public string Lastname { get; set; }
    }


    public class StateBotSample : ActivityHandler
    {
        private ConversationState _conversationState;
        private UserState _userstate;
        public StateBotSample(ConversationState constate,UserState userState)
        {
            _conversationState = constate;
            _userstate = userState;
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to Bot"), cancellationToken);
                }
            }
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var conprop = _conversationState.CreateProperty<ConvState>(nameof(ConvState));
            var conobject = await conprop.GetAsync(turnContext,()=>new ConvState());

            var userProp = _userstate.CreateProperty<Userprofile>(nameof(Userprofile));
            var userobject = await userProp.GetAsync(turnContext, () => new Userprofile());

            //main logic

            switch(conobject.conTrack)
            {
                case ConTrack.Name:
                    await turnContext.SendActivityAsync("Hello , Enter your name");
                    conobject.conTrack = ConTrack.LastName;
                    break;
                case ConTrack.LastName:
                    userobject.username = turnContext.Activity.Text;
                    await turnContext.SendActivityAsync("Hello , Enter your Last Name");
                    conobject.conTrack = ConTrack.Complete;
                    break;
                case ConTrack.Complete:
                    userobject.Lastname = turnContext.Activity.Text;
                    conobject.conTrack = ConTrack.Name;
                    break;
            }


            //if(string.IsNullOrEmpty(userobject.username))
            //{
            //    if(!conobject.username)
            //    {
            //        await turnContext.SendActivityAsync("Hello , Enter your name");
            //        conobject.username = true;
            //    }
            //    else
            //    {
            //        userobject.username = turnContext.Activity.Text;
            //    }
            //}

            await _conversationState.SaveChangesAsync(turnContext,false,cancellationToken);
            await _userstate.SaveChangesAsync(turnContext, false, cancellationToken);

        }
    }
}
