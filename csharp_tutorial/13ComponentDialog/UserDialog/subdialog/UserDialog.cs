using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace UserDialog.subdialog
{
    public class UserDialog : ComponentDialog
    {
        public UserDialog() : base(nameof(UserDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<int>("NumberPrompt"));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                GetUserName,
                GetMobileNo,
                ConformDlg
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ConformDlg(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["Phone"] = stepContext.Result;
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> GetMobileNo(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["UserName"] = stepContext.Result;

            var promptUser = stepContext.PromptAsync("NumberPrompt", new PromptOptions()
            {
                Prompt = MessageFactory.Text("Please enter your phone Number"),
                RetryPrompt = MessageFactory.Text("Hello , enter the valid number")
            }, cancellationToken);

            return await promptUser;
        }

        private async Task<DialogTurnResult> GetUserName(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptUser = stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions()
            {
                Prompt = MessageFactory.Text($"Please enter your name"),

            }, cancellationToken);

            return await promptUser;
        }
    }
}
