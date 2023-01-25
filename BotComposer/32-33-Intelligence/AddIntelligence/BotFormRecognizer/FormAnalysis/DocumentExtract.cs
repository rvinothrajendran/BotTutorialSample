using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using System;
using System.Threading.Tasks;

namespace BotFormRecognizer.FormAnalysis
{
    internal class DocumentExtract
    {
        readonly DocumentAnalysisClient client;
        public DocumentExtract(string key, string endpoint)
        {
            var credential = new AzureKeyCredential(key);
            client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        }

        public async Task<string> Extract(string url)
        {
            var documentUrl = new Uri(url);

            var operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-businessCard", documentUrl);

            var businessCards = operation.Value;

            return ExtractValue(businessCards);
        }

        private string ExtractValue(AnalyzeResult businessCards)
        {
            var contactName = string.Empty;

            businessCards.Documents[0].Fields.TryGetValue("ContactNames", out var contactNamesField);

            if (contactNamesField != null)
            {
                foreach (var contactNameField in contactNamesField.Value.AsList())
                {
                    if (contactNameField.FieldType == DocumentFieldType.Dictionary)
                    {
                        var contactNameFields = contactNameField.Value.AsDictionary();

                        if (contactNameFields.TryGetValue("FirstName", out var firstNameField))
                        {
                            if (firstNameField.FieldType == DocumentFieldType.String)
                            {
                                contactName = $"  First Name: '{firstNameField.Value.AsString()}'";
                            }
                        }

                        if (contactNameFields.TryGetValue("LastName", out var lastNameField))
                        {
                            if (lastNameField.FieldType == DocumentFieldType.String)
                            {
                                contactName += $"  Last Name: '{lastNameField.Value.AsString()}'";
                            }
                        }
                    }
                }
            }

            return contactName;
        }
    }
}
