using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;

namespace AdaptiveDialogInput.Helper
{
    public class DialogBot<T> : ActivityHandler where T : Dialog
    {
        private readonly DialogManager _dialogManager;
        protected readonly ILogger Logger;
        public DialogBot(T rootDialog, ILogger<DialogBot<T>> logger)
        {
            Logger = logger;

            _dialogManager = new DialogManager(rootDialog);
            //_dialogManager.UseResourceExplorer(new ResourceExplorer());
            //_dialogManager.UseLanguageGeneration();
        }


        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Running dialog with Activity.");
            await _dialogManager.OnTurnAsync(turnContext, cancellationToken: cancellationToken).ConfigureAwait(false);

        }
    }
}
