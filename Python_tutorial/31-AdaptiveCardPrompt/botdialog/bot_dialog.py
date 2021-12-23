from botbuilder.core import TurnContext,ActivityHandler,ConversationState,MessageFactory,CardFactory
from botbuilder.schema import  Activity, ActivityTypes
from botbuilder.dialogs import DialogSet,WaterfallDialog,WaterfallStepContext
from botbuilder.dialogs.prompts import TextPrompt,NumberPrompt,PromptOptions, prompt
from botbuilder.sangam.dialogs.prompt.adaptive import AdaptiveCardPrompt

class BotDialog(ActivityHandler):
    def __init__(self,conversation:ConversationState):
        self.con_statea = conversation
        self.state_prop = self.con_statea.create_property("dialog_set")
        self.dialog_set = DialogSet(self.state_prop)
        self.dialog_set.add(AdaptiveCardPrompt("adaptive_prompt"))
        self.dialog_set.add(WaterfallDialog("main_dialog",[self.adaptive_card_prompt,self.display_card]))

    async def adaptive_card_prompt(self,waterfall_step:WaterfallStepContext):
        card = {  
                    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",  
                    "type": "AdaptiveCard",  
                    "version": "1.0",  
                    "body": [  
                        {  
                        "type": "TextBlock",  
                        "text": "User Form"  
                        },  
                        {  
                        "type": "Input.Text",  
                        "id": "firstName",  
                        "placeholder": "What is your first name?"  
                        },  
                        {  
                        "type": "Input.Text",  
                        "id": "lastName",  
                        "placeholder": "What is your last name?"  
                        }  
                    ],  
                    "actions": [  
                        {  
                        "type": "Action.Submit",  
                        "title": "Submit"  
                        
                        }  
                    ]  
                }

        messagae = Activity(type=ActivityTypes.message,attachments=[CardFactory.adaptive_card(card)],
        text="Please enter your details")

        prompt_options = PromptOptions(prompt=messagae)
        prompt_options.type = ActivityTypes.message

        return await waterfall_step.prompt("adaptive_prompt",prompt_options)        

    async def display_card(self,waterfall_step:WaterfallStepContext):
        information = ""
        if waterfall_step.result:
            card_info = waterfall_step._turn_context.activity.value
            for key,value in card_info.items():
                information += f"{key} : {value}\n"

        await waterfall_step._turn_context.send_activity(information)
        
    async def on_turn(self,turn_context:TurnContext):
        dialog_context = await self.dialog_set.create_context(turn_context)

        if(dialog_context.active_dialog is not None):
            await dialog_context.continue_dialog()
        else:
            await dialog_context.begin_dialog("main_dialog")
        
        await self.con_statea.save_changes(turn_context)
    