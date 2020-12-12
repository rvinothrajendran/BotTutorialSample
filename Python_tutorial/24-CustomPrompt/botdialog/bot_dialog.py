import pathlib
import sys
from botbuilder.core import TurnContext,ActivityHandler,ConversationState,MessageFactory
from botbuilder.dialogs import DialogSet,WaterfallDialog,WaterfallStepContext
from botbuilder.dialogs.prompts import TextPrompt,NumberPrompt,PromptOptions,PromptValidatorContext

current = pathlib.Path(__file__).parent.parent
libpath = current.joinpath("prompt")
sys.path.append(str(libpath))

from email_prompt import EmailPrompt


class BotDialog(ActivityHandler):
    def __init__(self,conversation:ConversationState):
        self.con_statea = conversation
        self.state_prop = self.con_statea.create_property("dialog_set")
        self.dialog_set = DialogSet(self.state_prop)
        self.dialog_set.add(EmailPrompt("email_prompt"))
        self.dialog_set.add(WaterfallDialog("main_dialog",[self.FindEmailPrompt,self.Completed]))
             
     
    async def FindEmailPrompt(self,waterfall_step:WaterfallStepContext):
        return await waterfall_step.prompt("email_prompt",PromptOptions(prompt=MessageFactory.text("Please enter email with Text")))

       
    async def Completed(self,waterfall_step:WaterfallStepContext):
        email = waterfall_step.result
        await waterfall_step._turn_context.send_activity(email)
        return await waterfall_step.end_dialog()
        
    async def on_turn(self,turn_context:TurnContext):
        dialog_context = await self.dialog_set.create_context(turn_context)

        if(dialog_context.active_dialog is not None):
            await dialog_context.continue_dialog()
        else:
            await dialog_context.begin_dialog("main_dialog")
        
        await self.con_statea.save_changes(turn_context)
    