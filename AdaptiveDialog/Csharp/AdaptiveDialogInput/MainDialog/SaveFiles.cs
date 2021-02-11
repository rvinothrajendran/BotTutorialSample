using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace AdaptiveDialogInput.MainDialog
{
    public class SaveFiles : Dialog
    {
        public StringExpression FilesInfo;
        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var status = string.Empty;
            try
            {
                var input = FilesInfo?.GetValue(dc.State);

                if (!string.IsNullOrEmpty(input))
                {
                    var imageList = JsonConvert.DeserializeObject<List<SaveImage>>(input);

                    if (imageList?.Count > 0)
                    {
                        foreach (var saveImage in imageList)
                        {
                            var webClient = new WebClient();
                            webClient.DownloadFileAsync(saveImage.ContentUrl, saveImage.Name);
                        }

                        status = "successfully";
                    }
                    else
                    {
                        status = "Failed";
                    }

                }
            }
            catch (Exception e)
            {
                status = e.Message;
            }
            

            await dc.Context.SendActivityAsync(MessageFactory.Text($"Files upload {status}"), cancellationToken);
            
            return await dc.EndDialogAsync(null, cancellationToken);

        }
    }

    public partial class SaveImage
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("contentUrl")]
        public Uri ContentUrl { get; set; }

        [JsonProperty("content")]
        public object Content { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("thumbnailUrl")]
        public object ThumbnailUrl { get; set; }
    }
}
