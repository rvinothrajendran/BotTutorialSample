using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;

namespace AdaptiveCard1.BotDialog
{
    public class RootDialog : ComponentDialog
    {
        public RootDialog() : base(nameof(RootDialog))
        {
           // var templates = Templates.ParseFile(Path.Combine(".", "Dialogs", "RootDialog.lg"));

            var rootDialog = new AdaptiveDialog
            {
                Recognizer = CreateRecognizer(),
                Triggers = new List<OnCondition>
                {
                    new OnIntent()
                    {
                        Intent = "book",
                        Actions = new List<Dialog>() {new SendActivity("Hey am Book Actions")}
                    },

                    new OnUnknownIntent()
                    {
                        Actions = new List<Dialog>() {new SendActivity("Sorry , not understand")}
                    }


                }
            };

            AddDialog(rootDialog);

            InitialDialogId = nameof(AdaptiveDialog);
        }


        private Recognizer CreateRecognizer()
        {
            var reg = new RegexRecognizer
            {
                Intents = new List<IntentPattern>
                {
                    new IntentPattern("book", "(?i)book"),
                    new IntentPattern("weather", "(?i)weather")
                }
            };
            return reg;
        }
    }
}