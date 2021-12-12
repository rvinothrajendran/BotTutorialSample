using System.Collections.Concurrent;
using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace TestProactive
{
    //TestProactive
    public class TestProactive : Dialog
    {
        public TestProactive([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) : base()
        {
            RegisterSourceLocation(sourceFilePath, sourceLineNumber);
        }

        [JsonProperty("$Kind")]
        public const string Kind = "TestProactive";

        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        public static readonly ConcurrentDictionary<string, ConversationReference> ConversationReferences =
            new ConcurrentDictionary<string, ConversationReference>();

        public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = AddConversationReference(dc.Context.Activity);

            if (ResultProperty != null)
            {
                dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
            }

            return dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        }

        private string AddConversationReference(Activity activity)
        {

            var conversationReference = activity.GetConversationReference();

            var userInfo = ConversationReferences.AddOrUpdate(conversationReference.User.Id,
                conversationReference, (key, newValue) => conversationReference);

            if (userInfo != null)
                return userInfo.User.Id;

            return "User Information is missing";
        }
    }
}
