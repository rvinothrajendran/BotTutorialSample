# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

from botbuilder.core import ActivityHandler, TurnContext
from botbuilder.sangamam.cognitive.text import translate
from botbuilder.schema import ChannelAccount
from botbuilder.sangamam.cognitive.text.translate import TranslateSettings,TranslateService,TranslateResponse, translate_response

class MyBot(ActivityHandler):
    # See https://aka.ms/about-bot-activity-message to learn more about the message and other activity types.
    def __init__(self,key : str) -> None:
        super().__init__()
        self.key = key
        self.settings = TranslateSettings()
        self.settings.subscription_key = self.key
        self.settings.subscription_region = "westeurope"
        self.settings.from_lang = "en"
        self.settings.to_lang = ["fr","de","ta","ja"]
        self.service = TranslateService(self.settings)

        
       
    async def on_message_activity(self, turn_context: TurnContext):

        response = self.service.translate_text(turn_context.activity.text)

        if response.status_code == 200:
            for translate in response.output:
                text = "Lang : " + translate.lang + " ,Text :" + translate.text
                await turn_context.send_activity(text)
        else:
            await turn_context.send_activity("Error : " + response.status_message)        

    async def on_members_added_activity(
        self,
        members_added: ChannelAccount,
        turn_context: TurnContext
    ):
        for member_added in members_added:
            if member_added.id != turn_context.activity.recipient.id:
                await turn_context.send_activity("Hello and welcome!")
