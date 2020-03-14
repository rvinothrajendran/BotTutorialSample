from flask import Flask,request,Response
from botbuilder.schema import Activity
from botbuilder.core import (
  BotFrameworkAdapter,
  BotFrameworkAdapterSettings,
  ConversationState,
  UserState,
  MemoryStorage
  )
import asyncio
from bot import StateBot

from botbuilder.azure import CosmosDbConfig,CosmosDbStorage

app = Flask(__name__)
loop = asyncio.get_event_loop()


botadaptersettings = BotFrameworkAdapterSettings("","")
botadapter = BotFrameworkAdapter(botadaptersettings)

memstore = MemoryStorage()
constate = ConversationState(memstore)
#userstate = UserState(memstore)

key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
cosconfig = CosmosDbConfig("https://localhost:8081",key,"userprofile","userinfo")
cosdbStore = CosmosDbStorage(cosconfig)

sbot = StateBot(constate,cosdbStore)

@app.route("/api/messages",methods=["POST"])
def messages():
    if "application/json" in request.headers["content-type"]:
      jsonmessage = request.json
    else:
      return Response(status=415)

    activity = Activity().deserialize(jsonmessage)

    async def turn_call(turn_context):
        await sbot.on_turn(turn_context)

    task = loop.create_task(botadapter.process_activity(activity,"",turn_call))
    loop.run_until_complete(task)

if __name__ == '__main__':
    app.run('localhost',3978)

