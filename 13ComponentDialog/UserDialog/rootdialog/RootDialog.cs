using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace UserDialog.rootdialog
{
    public class RootDialog<T> : ActivityHandler where T : Dialog
    {
        BotState convState;
        BotState userState;
        Dialog dlgRoot;
        public RootDialog(ConversationState conversation,UserState userstate, T Dialog)
        {
            convState = conversation;
            userState = userstate;
            dlgRoot = Dialog;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            await convState.SaveChangesAsync(turnContext, false, cancellationToken);
            await userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected async override Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await dlgRoot.RunAsync(turnContext, convState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
        }
    }
}
