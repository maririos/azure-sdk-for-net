// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.Samples
{
    [LiveOnly]
    public partial class DocumentTranslationSamples : SamplesBase<DocumentTranslationTestEnvironment>
    {
        [Test]
        public async Task DocumentStatusAsync()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            string sourceUrl = TestEnvironment.SourceUrl;
            string targetUrl = TestEnvironment.TargetUrl;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var inputs = new List<TranslationOperationConfiguration>()
                {
                    new TranslationOperationConfiguration(new SourceConfiguration(sourceUrl)
                        {
                            Language = "en"
                        },
                    new List<TargetConfiguration>()
                        {
                            new TargetConfiguration(targetUrl, "it")
                        })
                    {
                        StorageType = StorageInputType.Folder
                    }
                };

            DocumentTranslationOperation operation = await client.StartTranslationAsync(inputs);

            // get not finished documents
            List<string> documentIds = new List<string>();
            AsyncPageable<DocumentStatusDetail> documents =  operation.GetDocumentsStatusAsync();

            await foreach (DocumentStatusDetail docStatus in documents)
            {
                if (docStatus.Status == TranslationStatus.NotStarted || docStatus.Status == TranslationStatus.Running)
                {
                    documentIds.Add(docStatus.Id);
                }
                else
                {
                    Console.WriteLine($"Document {docStatus.Url} completed with status ${docStatus.Status}");
                }
            }

            TimeSpan pollingInterval = new TimeSpan(1000);

            while (documentIds.Count > 0)
            {
                await Task.Delay(pollingInterval);
                for (int i = documentIds.Count - 1; i >= 0; i--)
                {
                    Response<DocumentStatusDetail> status = await operation.GetDocumentStatusAsync(documentIds[i]);
                    if (status.Value.Status != TranslationStatus.NotStarted && status.Value.Status != TranslationStatus.Running)
                    {
                        Console.WriteLine($"Document {status.Value.Url} completed with status ${status.Value.Status}");
                        documentIds.RemoveAt(i);
                    }
                }
            }
        }
    }
}
