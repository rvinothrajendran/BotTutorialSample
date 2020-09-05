using System.Collections.Generic;
using System.IO;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;
using Microsoft.Bot.Builder.LanguageGeneration;
using Microsoft.Extensions.Localization.Internal;

namespace Part02LGTemplate.RootDialog
{
    public class RootDialog : ComponentDialog
    {
        public RootDialog() : base(nameof(RootDialog))
        {
            string[] path = {".", "Template", "simple.lg"};
            var fullpath = Path.Combine(path);
            
            var rootDialog = new AdaptiveDialog
            {
                Generator = new TemplateEngineLanguageGenerator(Templates.ParseFile(fullpath)),
                Recognizer = CreateRecognizer(),
                Triggers = new List<OnCondition>
                {
                    new OnIntent()
                    {
                        Intent = "book",
                        Actions = new List<Dialog>() {new SendActivity("${Book()}")}
                    },

                    new OnIntent()
                    {
                        Intent = "weather",
                        Actions = new List<Dialog>() {new SendActivity("${Weather()}") }
                    },

                    new OnUnknownIntent()
                    {
                        Actions = new List<Dialog>() {new SendActivity("${Unknown()}") }
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
