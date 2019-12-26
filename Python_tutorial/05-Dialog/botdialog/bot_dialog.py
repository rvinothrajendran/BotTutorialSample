from botbuilder.core import TurnContext,ActivityHandler,ConversationState,MessageFactory
from botbuilder.dialogs import DialogSet,WaterfallDialog,WaterfallStepContext
from botbuilder.dialogs.prompts import TextPrompt,NumberPrompt,PromptOptions

class BotDialog(ActivityHandler):
    def __init__(self,conversation:ConversationState):
        self.con_statea = conversation
        self.state_prop = self.con_statea.create_property("dialog_set")
        self.dialog_set = DialogSet(self.state_prop)
        self.dialog_set.add(TextPrompt("text_prompt"))
        self.dialog_set.add(NumberPrompt("number_prompt"))
        self.dialog_set.add(WaterfallDialog("main_dialog",[self.GetUserName,self.GetMobileNumber,self.GetEmailId,self.Completed]))

    async def GetUserName(self,waterfall_step:WaterfallStepContext):
        return await waterfall_step.prompt("text_prompt",PromptOptions(prompt=MessageFactory.text("Please enter the name")))

    async def GetMobileNumber(self,waterfall_step:WaterfallStepContext):
        name = waterfall_step._turn_context.activity.text
        waterfall_step.values["name"] = name
        return await waterfall_step.prompt("number_prompt",PromptOptions(prompt=MessageFactory.text("Please enter the mobile no")))
        
    async def GetEmailId(self,waterfall_step:WaterfallStepContext):
        mobile = waterfall_step._turn_context.activity.text
        waterfall_step.values["mobile"] = mobile
        return await waterfall_step.prompt("text_prompt",PromptOptions(prompt=MessageFactory.text("PLease enter the email id")))
        
    async def Completed(self,waterfall_step:WaterfallStepContext):
        email = waterfall_step._turn_context.activity.text
        waterfall_step.values["email"] = email
        name = waterfall_step.values["name"]
        mobile = waterfall_step.values["mobile"]
        mail = waterfall_step.values["email"] 
        profileinfo = f"name : {name} , Email : {mail} , mobile {mobile}"
        await waterfall_step._turn_context.send_activity(profileinfo)
        return await waterfall_step.end_dialog()
        
    async def on_turn(self,turn_context:TurnContext):
        dialog_context = await self.dialog_set.create_context(turn_context)

        if(dialog_context.active_dialog is not None):
            await dialog_context.continue_dialog()
        else:
            await dialog_context.begin_dialog("main_dialog")
        
        await self.con_statea.save_changes(turn_context)
    