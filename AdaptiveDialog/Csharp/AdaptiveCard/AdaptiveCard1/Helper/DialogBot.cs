using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace AdaptiveCard1.Helper
{
    public class DialogBot<T> : ActivityHandler where T: Dialog
    {
        protected readonly BotState ConversationBotState;
        protected readonly BotState UserBotState;
        private readonly Dialog _dialog;

        public DialogBot(ConversationState conversationState,UserState userState,T dialog)
        {
            ConversationBotState = conversationState;
            UserBotState = userState;
            _dialog = dialog;
        }



        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = new CancellationToken())
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            await ConversationBotState.SaveChangesAsync(turnContext,false, cancellationToken);
            await UserBotState.SaveChangesAsync(turnContext, false, cancellationToken);

        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await _dialog.RunAsync(turnContext, ConversationBotState.CreateProperty<DialogState>(nameof(DialogState)),
                cancellationToken);
        }
    }
}
