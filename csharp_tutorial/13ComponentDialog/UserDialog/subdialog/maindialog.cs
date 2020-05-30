using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UserDialog.subdialog
{
    public class maindialog : ComponentDialog
    {
        public maindialog(): base(nameof(maindialog))
        {
            AddDialog(new MovieListDialog());
            AddDialog(new UserDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                StartMovieDlg,
                StartUserDlg,
                ShowResult
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }        

        private async Task<DialogTurnResult> StartMovieDlg(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(MovieListDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> StartUserDlg(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(UserDialog), stepContext.Result, cancellationToken);           
        }
        private async Task<DialogTurnResult> ShowResult(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(null, cancellationToken);           
        }
    }
}
