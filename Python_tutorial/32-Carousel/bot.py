# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

from botbuilder.core import ActivityHandler, TurnContext,CardFactory
from botbuilder.core.message_factory import MessageFactory
from botbuilder.schema import ChannelAccount
from botbuilder.schema import Attachment,HeroCard,MediaUrl,CardAction,ActionTypes,CardImage,ReceiptCard,Fact,ReceiptItem


class MyBot(ActivityHandler):
    # See https://aka.ms/about-bot-activity-message to learn more about the message and other activity types.

    async def on_message_activity(self, turn_context: TurnContext):
        list_items = []
        list_items.append(self.create_receipt_card())
        list_items.append(self.create_hero_card())
        list_items.append(self.create_hero_card())
        list_items.append(self.create_receipt_card())
        activity = MessageFactory.carousel(list_items)
        await turn_context.send_activity(activity)


    async def on_members_added_activity(
        self,
        members_added: ChannelAccount,
        turn_context: TurnContext
    ):
        for member_added in members_added:
            if member_added.id != turn_context.activity.recipient.id:
                await turn_context.send_activity("Hello and welcome!")


    def create_hero_card(self) -> Attachment:
      herocard = HeroCard(title="Sample of Hero Card in Bot using Python",
      images=[CardImage(url="https://upload.wikimedia.org/wikipedia/commons/thumb/4/49/Seattle_monorail01_2008-02-25.jpg/1024px-Seattle_monorail01_2008-02-25.jpg")],
      buttons=[CardAction(type=ActionTypes.open_url,title="Open Url",value="https://dev.botframework.com/")])
      return CardFactory.hero_card(herocard)
    
    def create_receipt_card(self) -> Attachment:
      card = ReceiptCard()
      card.title = "Receipt Card - Sample"
      card.facts = [
         Fact(key="Bill NO",value="2001"),
         Fact(key="Payment Mode",value="Cash")
      ]
      card.items = [
         ReceiptItem(title="Bot Framework book",price="20Euro",quantity=1,image=CardImage(url="https://pypi.org/static/images/logo-small.6eef541e.svg")),
         ReceiptItem(title="Python Book",price="100 Euro",quantity=5,image=CardImage(url="https://pypi.org/static/images/logo-small.6eef541e.svg"))
      ]
      card.vat = "0.2"
      card.tax = "12%"
      card.total = "180"
      return CardFactory.receipt_card(card) 