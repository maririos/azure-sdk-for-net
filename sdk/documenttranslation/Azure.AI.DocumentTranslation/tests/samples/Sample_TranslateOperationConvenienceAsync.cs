// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
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
        public async Task TranslateOperationConvenienceTestAsync()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            string sourceUrl = TestEnvironment.SourceUrl;
            string targetUrl = TestEnvironment.TargetUrl;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            Response<string> operation = await client.StartBatchTranslationAsync(new Uri(sourceUrl), "es", new Uri(targetUrl), "en");

            Response<OperationStatusDetail> response = await client.WaitForOperationCompletionAsync(operation.Value);

            Console.WriteLine($"  Status: {response.Value.Status}");
            Console.WriteLine($"  Created on: {response.Value.CreatedOn}");
            Console.WriteLine($"  Last modified: {response.Value.LastModified}");
            Console.WriteLine($"  Total documents: {response.Value.TotalDocuments}");
            Console.WriteLine($"    Succeeded: {response.Value.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {response.Value.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {response.Value.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {response.Value.DocumentsNotStarted}");

            // Get Status of documents
            AsyncPageable<DocumentStatusDetail> documents = client.GetStatusesOfDocumentsAsync(operation.Value);

            await foreach (DocumentStatusDetail document in documents)
            {
                Console.WriteLine($"Document with Id: {document.Id}");
                Console.WriteLine($"  Status:{document.Status}");
                if (document.Status == DocumentTranslationStatus.Succeeded)
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
