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
        public async Task TranslateOperationAsync()
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

            var operation = await client.StartBatchTranslationAsync(inputs);

            Response<AsyncPageable<DocumentStatusDetail>> response = await operation.WaitForCompletionAsync();
            var docsEnumerator = response.Value.GetAsyncEnumerator();

            while (await docsEnumerator.MoveNextAsync())
            {
                if (docsEnumerator.Current.Status == DocumentTranslationOperationStatus.Succeeded)
                {
                    Console.WriteLine($"Document {docsEnumerator.Current.Path} succedded");
                }
                else
                {
                    Console.WriteLine($"Document {docsEnumerator.Current.Path} failed");
                }
            }
        }
    }
}
