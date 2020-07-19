from botbuilder.core import TurnContext,ActivityHandler,MessageFactory,CardFactory
from botbuilder.schema import Attachment,AudioCard,MediaUrl,ThumbnailUrl

class SampleAudioCard(ActivityHandler):
   def __init__(self):
      pass

   async def on_message_activity(self,turn_context:TurnContext):
      cardAtt = self.create_audio_card()
      msg_activity = MessageFactory.attachment(cardAtt)
      await turn_context.send_activity(msg_activity)
   
   def create_audio_card(self) -> Attachment:
      card = AudioCard()
      card.media = [MediaUrl(url="https://wavlist.com/wav/apli-airconditioner.wav")]
      card.subtitle = "Bot Framework Audio Card using python"
      card.title = "Lets play an audio"
      card.image = ThumbnailUrl(url="https://pypi.org/static/images/logo-small.6eef541e.svg")
      return CardFactory.audio_card(card)
      
      

   