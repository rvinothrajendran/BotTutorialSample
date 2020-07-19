from botbuilder.core import TurnContext,ActivityHandler,MessageFactory,CardFactory
from botbuilder.schema import Attachment,MediaUrl,ThumbnailCard,CardImage,ThumbnailUrl


class SampleThumbNailCard(ActivityHandler):
   def __init__(self):
      pass

   async def on_message_activity(self,turn_context:TurnContext):
      cardAtt = self.thumbnail_card()
      msg_activity = MessageFactory.attachment(cardAtt)
      await turn_context.send_activity(msg_activity)
      

   def thumbnail_card(self) -> Attachment:
     card = ThumbnailCard()
     #card.images = [CardImage(url="https://pypi.org/static/images/logo-large.72ad8bf1.svg")]
     card.images = [ThumbnailUrl(url="https://pypi.org/static/images/logo-large.72ad8bf1.svg")]
     card.title = "Bot builder sample card"
     card.subtitle = "SDK Version 4.9"
     card.text = "Thumbnail card is the one of the card type in Bot Framework"
     return CardFactory.thumbnail_card(card)

   

   