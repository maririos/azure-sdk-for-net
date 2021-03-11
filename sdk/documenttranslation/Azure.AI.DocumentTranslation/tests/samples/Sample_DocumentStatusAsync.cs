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
            Uri sourceUrl = new Uri(TestEnvironment.SourceBlobContainerSas);
            Uri targetUrl = new Uri(TestEnvironment.TargetBlobContainerSas);

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            JobStatusDetail job = await client.CreateTranslationJobForAzureBlobsAsync(sourceUrl, targetUrl, "it");

            var documentscompleted = new HashSet<string>();
            TimeSpan pollingInterval = new TimeSpan(1000);

            do
            {
                await foreach (DocumentStatusDetail docStatus in client.GetDocumentsStatusAsync(job.Id))
                {
                    if (documentscompleted.Contains(docStatus.Id))
                        continue;
                    if (docStatus.HasCompleted)
                    {
                        documentscompleted.Add(docStatus.Id);
                        Console.WriteLine($"Document {docStatus.Url} completed with status ${docStatus.Status}");
                    }
                }

                await Task.Delay(pollingInterval);
            } while (job.DocumentsTotal != documentscompleted.Count);
        }
    }
}
