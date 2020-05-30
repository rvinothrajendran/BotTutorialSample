using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UserDialog.subdialog
{
    public class MovieListDialog : ComponentDialog
    {
        public MovieListDialog() : base(nameof(MovieListDialog))
        {
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                SelectMovie,
                SelectTime,
                Final
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> Final(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is FoundChoice result)
            {
                stepContext.Values["showtime"] = result.Value;
            }

            var strmovie = $"Movie Name :{stepContext.Values["selectmovie"]} and Movie Time : {stepContext.Values["showtime"]}";

            return await stepContext.EndDialogAsync(strmovie, cancellationToken);
        }

        private async Task<DialogTurnResult> SelectTime(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is FoundChoice result)
            {
                stepContext.Values["selectmovie"] = result.Value;
            }

            var pmoptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Hey , Please select show time"),
                Choices = GetShowTime()
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), pmoptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SelectMovie(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var pmoptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Hey , Please select the movie"),
                Choices = GetMovieList()
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), pmoptions, cancellationToken);
        }

        public List<Choice> GetMovieList()
        {
            var lstChoice = new List<Choice>
            {
                new Choice() { Value = "Movie 1" },
                new Choice() { Value = "Movie 2" },
                new Choice() { Value = "Movie 3" }
            };
            return lstChoice;
        }

        public List<Choice> GetShowTime()
        {
            var lstChoice = new List<Choice>
            {
                new Choice() { Value = "10.00 am" },
                new Choice() { Value = "1.00 pm" },
                new Choice() { Value = "4.00 pm" },
                new Choice() { Value = "7.00 pm" }
            };
            return lstChoice;
        }

    }       
}

   

