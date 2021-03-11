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
        public async Task TranslateJobAsync()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            Uri sourceUrl = new Uri(TestEnvironment.SourceBlobContainerSas);
            Uri targetUrl = new Uri(TestEnvironment.TargetBlobContainerSas);

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            JobStatusDetail job = await client.CreateTranslationJobForAzureBlobsAsync(sourceUrl, targetUrl, "it");

            JobStatusDetail jobStatus = await client.WaitForJobCompletionAsync(job.Id);

            Console.WriteLine($"  Status: {jobStatus.Status}");
            Console.WriteLine($"  Created on: {jobStatus.CreatedOn}");
            Console.WriteLine($"  Last modified: {jobStatus.LastModified}");
            Console.WriteLine($"  Total documents: {jobStatus.DocumentsTotal}");
            Console.WriteLine($"    Succeeded: {jobStatus.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {jobStatus.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {jobStatus.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {jobStatus.DocumentsNotStarted}");

            await foreach (DocumentStatusDetail document in client.GetDocumentsStatusAsync(job.Id))
            {
                Console.WriteLine($"Document with Id: {document.Id}");
                Console.WriteLine($"  Status:{document.Status}");
                if (document.Status == TranslationStatus.Succeeded)
                {
                    Console.WriteLine($"  Location: {document.Url}");
                    Console.WriteLine($"  Translated to language: {document.TranslateTo}.");
                }
                else
                {
                    Console.WriteLine($"  Error Code: {document.Error.Code}");
                    Console.WriteLine($"  Message: {document.Error.Message}");
                }
            }
        }
    }
}
