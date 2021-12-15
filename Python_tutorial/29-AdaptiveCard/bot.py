# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

from botbuilder.core import ActivityHandler, TurnContext, CardFactory
from botbuilder.schema import ChannelAccount, Attachment, Activity, ActivityTypes
from adaptive_table import AdaptiveTable


class MyBot(ActivityHandler):
    # See https://aka.ms/about-bot-activity-message to learn more about the message and other activity types.

    async def on_message_activity(self, turn_context: TurnContext):
        await turn_context.send_activity(f"You said '{ turn_context.activity.text }'")

    async def on_members_added_activity(
        self,
        members_added: ChannelAccount,
        turn_context: TurnContext
    ):
        for member_added in members_added:
            if member_added.id != turn_context.activity.recipient.id:
               
               col_name = AdaptiveTable.create_column("Name",["Vinoth","Siva","Kumar"])
               col_city = AdaptiveTable.create_column("City",["Chennai","Bangalore","Hyderabad"])
               col_stat = AdaptiveTable.create_column("State",["TamilNadu","Karnataka","Telangana"])
               col_country = AdaptiveTable.create_column("Country",["India","India","India"])
               col_zip = AdaptiveTable.create_column("Zip",["600097","560037","500001"])
               col_phone = AdaptiveTable.create_column("Phone",["9888888888","9888888888","9888888888"]) 
               col_set = AdaptiveTable.create_column_list([col_name,col_city,col_stat,col_country,col_zip,col_phone])
               jsonObject = AdaptiveTable.prepare_json(col_set)
               message = Activity(type=ActivityTypes.message)
               message.attachments = [ CardFactory.adaptive_card(jsonObject) ]
               message.text = "Adaptive Table"
               await turn_context.send_activity(message)
               await turn_context.send_activity("Hello and welcome!")
                
