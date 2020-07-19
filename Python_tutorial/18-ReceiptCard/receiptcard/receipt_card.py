from botbuilder.core import TurnContext,ActivityHandler,MessageFactory,CardFactory
from botbuilder.schema import Attachment,MediaUrl,CardImage,CardAction,ActionTypes,ReceiptCard,Fact,ReceiptItem


class SampleReceiptCard(ActivityHandler):
   def __init__(self):
      pass

   async def on_message_activity(self,turn_context:TurnContext):
      cardAtt = self.create_receipt_card()
      msg_activity = MessageFactory.attachment(cardAtt)
      await turn_context.send_activity(msg_activity)
   
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


   