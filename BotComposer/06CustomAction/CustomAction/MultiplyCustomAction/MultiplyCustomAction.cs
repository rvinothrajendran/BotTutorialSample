using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplyCustomAction
{
    //Multiply of two numbers
    public class MultiplyCustomAction : Dialog
    {
        public MultiplyCustomAction([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) : base()
        {
            RegisterSourceLocation(sourceFilePath, sourceLineNumber);
        }

        [JsonProperty("$Kind")]
        public const string Kind = "MultiplyCustomAction";

        [JsonProperty("Number1")]
        public NumberExpression Number1 { get; set; }

        [JsonProperty("Number2")]
        public NumberExpression Number2 {  get; set; }  


        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {

            var num1 = Number1.GetValue(dc.State);
            var num2 = Number2.GetValue(dc.State);

            var result = Convert.ToInt32(num1) * Convert.ToInt32(num2) ;

            if (ResultProperty != null)
            {
                dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
            }

            return dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        }
    }
}
