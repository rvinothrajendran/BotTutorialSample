using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BotFormRecognizer.DataStorage;
using BotFormRecognizer.FormAnalysis;

namespace BotFormRecognizer
{
    //Extract Form data
    public class BotFormRecognizer : Dialog
    {
        public BotFormRecognizer([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) : base()
        {
            RegisterSourceLocation(sourceFilePath, sourceLineNumber);
        }

        [JsonProperty("$Kind")]
        public const string Kind = "BotFormRecognizer";

        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        [JsonProperty("FileUrl")]
        public StringExpression FileUrl { get; set; }

        [JsonProperty("FileName")]
        public StringExpression FileName { get; set; }


        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = string.Empty;

            var fileUrl = FileUrl?.GetValue(dc.State);
            var fileName = FileName?.GetValue(dc.State);

            var store = new AzureStorage(Settings.ConnectionString, Settings.ContainerName);
            
            var filelocation = await store.UploadAsync(fileUrl, fileName);

            var document = new DocumentExtract(Settings.Key, Settings.Endpoint);

            result = await document.Extract(filelocation);


            if (ResultProperty != null)
            {
                dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
            }

            return await dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        }

    }

}
