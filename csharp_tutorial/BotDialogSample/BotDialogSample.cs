// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace BotDialogSample
{
    public class DialogSample : ActivityHandler
    {
        private StateAccessor _stateAccessor;
        private DialogSet _dialogSet;
        DialogContext dlgContext;

        private string DlgMain = "MainDlg";
        private string DlgUser = "UserName";
        private string DlgPhoneNumber = "PhoneNumber";

        public DialogSample(StateAccessor stateAccessor)
        {
            _stateAccessor = stateAccessor;
            PrepareDialogs();
        }


        private void PrepareDialogs()
        {
            _dialogSet = new DialogSet(_stateAccessor.DlgState);
            _dialogSet.Add(new TextPrompt(DlgUser));
            _dialogSet.Add(new NumberPrompt<int>(DlgPhoneNumber));

            var waterfallStep = new WaterfallStep[]
            {
                UserNameAsync,
                PhoneNumberAsync,
                CompleteAsync
            };

            _dialogSet.Add(new WaterfallDialog(DlgMain, waterfallStep));

        }

        private async Task<bool> IsMobileValid(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {

            if(promptContext.AttemptCount == 2)
            {
                await promptContext.Context.SendActivityAsync("Hello , You have reached the limit, Try again later..Bye Bye");
                await dlgContext.EndDialogAsync();
                return false;
            }

            if (promptContext.Recognized.Succeeded)
            {
                var count = Convert.ToString(promptContext.Recognized.Value).Length;

                if (count != 10)
                {
                    await promptContext.Context.SendActivityAsync("Hello , Please enter the valid moible number");
                    return false;
                }            
            }
            else
            {
                await promptContext.Context.SendActivityAsync("Hello , Please enter the valid number");
                return false;
            }

            return true;
        }

        private async Task<DialogTurnResult> CompleteAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var phoneNumber = stepContext.Result;

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> PhoneNumberAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userName = stepContext.Result;

            var promptUser = stepContext.PromptAsync(DlgPhoneNumber, new PromptOptions()
            {
                Prompt = MessageFactory.Text("Please enter your phone Number"),
                RetryPrompt = MessageFactory.Text("Hello , enter the valid number")
            }, cancellationToken);

            return await promptUser;
        }

        private async Task<DialogTurnResult> UserNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptUser = stepContext.PromptAsync(DlgUser, new PromptOptions()
            {
                Prompt = MessageFactory.Text("Please enter your name")
            }, cancellationToken);

            return await promptUser;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello world!"), cancellationToken);
                }
            }
        }

        protected async override Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            dlgContext = await _dialogSet.CreateContextAsync(turnContext, cancellationToken);

            if(dlgContext !=null && dlgContext.ActiveDialog is null)
            {
                await dlgContext.BeginDialogAsync(DlgMain, cancellationToken);
            }
            else if(dlgContext != null && dlgContext.ActiveDialog !=null)
            {
                await dlgContext.ContinueDialogAsync(cancellationToken);
            }
            await _stateAccessor.ConvState.SaveChangesAsync(turnContext,false, cancellationToken);

        }
    }
}
