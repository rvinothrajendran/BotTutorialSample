from botbuilder.core import TurnContext,ActivityHandler,MessageFactory,CardFactory
from botbuilder.schema import Attachment,MediaUrl,HeroCard,CardImage,CardAction,ActionTypes


class SampleAnimationCard(ActivityHandler):
   def __init__(self):
      pass

   async def on_message_activity(self,turn_context:TurnContext):
      cardAtt = self.create_hero_card()
      msg_activity = MessageFactory.attachment(cardAtt)
      await turn_context.send_activity(msg_activity)
      

   def create_hero_card(self) -> Attachment:
      herocard = HeroCard(title="Sample of Hero Card in Bot using Python",
      images=[CardImage(url="https://upload.wikimedia.org/wikipedia/commons/thumb/4/49/Seattle_monorail01_2008-02-25.jpg/1024px-Seattle_monorail01_2008-02-25.jpg")],
      buttons=[CardAction(type=ActionTypes.open_url,title="Open Url",value="https://dev.botframework.com/")])
      return CardFactory.hero_card(herocard)
   