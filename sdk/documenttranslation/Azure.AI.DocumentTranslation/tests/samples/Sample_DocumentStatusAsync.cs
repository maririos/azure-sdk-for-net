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

            var inputs = new List<BatchDocumentInput>()
                {
                    new BatchDocumentInput(new SourceInput(sourceUrl)
                        {
                            Language = "en"
                        },
                    new List<TargetInput>()
                        {
                            new TargetInput(targetUrl, "it")
                        })
                    {
                        StorageType = StorageInputType.Folder
                    }
                };

            DocumentTranslationOperation operation = await client.StartBatchTranslationAsync(inputs);

            // get first document
            AsyncPageable<DocumentStatusDetail> documents = operation.GetStatusesOfDocumentsAsync();
            IAsyncEnumerator<DocumentStatusDetail> docsEnumerator = documents.GetAsyncEnumerator();
            await docsEnumerator.MoveNextAsync();

            DocumentStatusDetail firstDocument = docsEnumerator.Current;

            TimeSpan pollingInterval = new TimeSpan(1000);

            Response<DocumentStatusDetail> docStatus = operation.GetDocumentStatus(firstDocument.Id);

            while (docStatus.Value.Status != DocumentTranslationStatus.Failed
                && docStatus.Value.Status != DocumentTranslationStatus.Succeeded)
            {
                await Task.Delay(pollingInterval);
                docStatus = operation.GetDocumentStatus(firstDocument.Id);
            }

            Console.WriteLine($"Document {firstDocument.Url} completed with status ${firstDocument.Status}");
        }
    }
}
