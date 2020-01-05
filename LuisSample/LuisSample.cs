// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace LuisSample
{
    public class MyLuisSample : ActivityHandler
    {
        private LuisHelper.LuisHelper _luisHelper;
        public MyLuisSample(LuisHelper.LuisHelper luisHelper)
        {
            _luisHelper = luisHelper;
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

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var result = await _luisHelper.RecognizeAsync<RestaurantLuis>(turnContext, cancellationToken);
            var topIntent = result.TopIntent().intent;

            switch (topIntent)
            {
                case RestaurantLuis.Intent.None:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_ChangeReservation:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_Confirm:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_DeleteReservation:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_FindReservationEntry:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_FindReservationWhen:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_FindReservationWhere:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_Reject:
                    break;
                case RestaurantLuis.Intent.RestaurantReservation_Reserve:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
