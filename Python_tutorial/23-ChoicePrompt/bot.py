from botbuilder.core import TurnContext,ActivityHandler,ConversationState,MessageFactory
from botbuilder.dialogs import DialogSet,WaterfallDialog,WaterfallStepContext
from botbuilder.dialogs.prompts import ChoicePrompt,PromptOptions
from botbuilder.dialogs.choices import Choice

class BotDialog(ActivityHandler):
    def __init__(self,conversation:ConversationState):
        self.con_statea = conversation
        self.state_prop = self.con_statea.create_property("dialog_set")
        self.dialog_set = DialogSet(self.state_prop)
        self.dialog_set.add(ChoicePrompt(ChoicePrompt.__name__))
        self.dialog_set.add(WaterfallDialog("main_dialog",[self.DisplayChoiceList,self.ReadResult]))

    async def DisplayChoiceList(self,waterfall_step:WaterfallStepContext):
        listofchoice = [Choice("MTech"),Choice("BTech"),Choice("MCA"),Choice("PhD")]
        return await waterfall_step.prompt((ChoicePrompt.__name__),
        PromptOptions(prompt=MessageFactory.text("Please select the your education"),choices=listofchoice))
       

    async def ReadResult(self,waterfall_step:WaterfallStepContext):
        choiceoption = waterfall_step.result.value
        await waterfall_step._turn_context.send_activity(MessageFactory.text(choiceoption))
        return await waterfall_step.end_dialog()

        
    async def on_turn(self,turn_context:TurnContext):
        dialog_context = await self.dialog_set.create_context(turn_context)

        if(dialog_context.active_dialog is not None):
            await dialog_context.continue_dialog()
        else:
            await dialog_context.begin_dialog("main_dialog")
        
        await self.con_statea.save_changes(turn_context)
    