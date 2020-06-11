from botbuilder.core import TurnContext,ActivityHandler,MessageFactory,CardFactory
from botbuilder.schema import AnimationCard,Attachment,MediaUrl


class SampleAnimationCard(ActivityHandler):
   def __init__(self):
      pass

   async def on_message_activity(self,turn_context:TurnContext):
      cardAtt = self.create_animation_card()
      msg_activity = MessageFactory.attachment(cardAtt)
      await turn_context.send_activity(msg_activity)
      

   def create_animation_card(self) -> Attachment:
     card = AnimationCard( media=[MediaUrl(url="http://i.giphy.com/Ki55RUbOV5njy.gif")],
     title="Sample Animation Card",subtitle="Hey am Bot Demo using python")
     return CardFactory.animation_card(card)
     
      