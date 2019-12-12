from botbuilder.core import Middleware,TurnContext
from typing import Callable,Awaitable
from botbuilder.schema import ActivityTypes

class Middleware1(Middleware):
    async def on_turn(self,turn_context:TurnContext,next:Callable[[TurnContext],Awaitable]):
        if turn_context.activity.type == ActivityTypes.message:
            await turn_context.send_activity("Hey am Middleware1")
            await next()
            await turn_context.send_activity("called after your bot")