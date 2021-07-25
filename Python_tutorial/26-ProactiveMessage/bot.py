# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

from botbuilder.core import ActivityHandler, TurnContext
from botbuilder.schema import ChannelAccount,ConversationReference
from typing import Dict

from botframework.connector.auth.credential_provider import SimpleCredentialProvider


class MyBot(ActivityHandler):
    def __init__(self, conversation: Dict[str,ConversationReference]):
        self.conver = conversation

    async def on_conversation_update_activity(self, turn_context: TurnContext):
        conversation = TurnContext.get_conversation_reference(turn_context.activity)
        self.conver[conversation.user.id] = conversation
        return await super().on_conversation_update_activity(turn_context)
         
    async def on_message_activity(self, turn_context: TurnContext):
        await turn_context.send_activity(f"You said '{ turn_context.activity.text }'")

    async def on_members_added_activity(
        self,
        members_added: ChannelAccount,
        turn_context: TurnContext
    ):
        for member_added in members_added:
            if member_added.id != turn_context.activity.recipient.id:
                await turn_context.send_activity("Hello and welcome!")
