using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptTerms
{
    //User Accept Terms Dialog for Bot Composer
    public class AcceptTerms : InputDialog
    {
        public AcceptTerms([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) : base()
        {
            RegisterSourceLocation(sourceFilePath, sourceLineNumber);

            acceptTermsAdaptive = new AcceptTermsAdaptive();
        }

        [JsonProperty("$Kind")]
        public const string Kind = "AcceptTerms";

        [JsonProperty("InformationProperty")]
        public StringExpression InformationProperty { get; set; }

        [JsonProperty("ActionNameProperty")]
        public StringExpression ActionNameProperty { get; set; }

        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        AcceptTermsAdaptive acceptTermsAdaptive;

        protected override Task<IActivity> OnRenderPromptAsync(DialogContext dc, InputState state, CancellationToken cancellationToken = default)
        {

            var attachment = PrepareInputSettings(dc);

            var staticTemplate = new StaticActivityTemplate((Activity)
                MessageFactory.Attachment(attachment));

            Prompt = staticTemplate;

            return base.OnRenderPromptAsync(dc, state, cancellationToken);
        }


        private Attachment PrepareInputSettings(DialogContext dc)
        {

            var agreeTerms = InformationProperty?.TryGetValue(dc.State).Value;

            if (string.IsNullOrEmpty(agreeTerms))
                agreeTerms = "I accept the terms and agreements";

            var actionName = ActionNameProperty?.GetValue(dc.State);

            if (string.IsNullOrEmpty(actionName))
                actionName = "Accept";

            return acceptTermsAdaptive.CreateAttachment(agreeTerms, actionName);

        }

        //public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    var result = string.Empty;

        //    if (ResultProperty != null)
        //    {
        //        dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
        //    }

        //    return dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        //}

        protected override Task<InputState> OnRecognizeInputAsync(DialogContext dc, CancellationToken cancellationToken)
        {

            var result = dc.State.GetValue<object>(VALUE_PROPERTY);

            if(result != null)
            {
                var selectedSet = JObject.FromObject(result).ToObject<Dictionary<string, string>>();

                if(selectedSet?.Count > 0 && selectedSet.TryGetValue("Accept",out var selectedValue))
                {
                    dc.State.SetValue(this.ResultProperty.GetValue(dc.State), selectedValue);
                    return Task.FromResult(InputState.Valid);
                }
            }

            return Task.FromResult(InputState.Invalid);

        }
    }
}
