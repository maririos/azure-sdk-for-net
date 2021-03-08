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
        public async Task TranslateOperationAsync()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            Uri sourceUrl = new Uri(TestEnvironment.SourceUrl);
            Uri targetUrl = new Uri(TestEnvironment.TargetUrl);

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            DocumentTranslationOperation operation = client.StartTranslationFromAzureBlobs(sourceUrl, targetUrl, "it");

            Response<AsyncPageable<DocumentStatusDetail>> operationResult = await operation.WaitForCompletionAsync();

            Console.WriteLine($"  Status: {operation.Status}");
            Console.WriteLine($"  Created on: {operation.CreatedOn}");
            Console.WriteLine($"  Last modified: {operation.LastModified}");
            Console.WriteLine($"  Total documents: {operation.DocumentsTotal}");
            Console.WriteLine($"    Succeeded: {operation.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {operation.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {operation.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {operation.DocumentsNotStarted}");

            await foreach (DocumentStatusDetail document in operationResult.Value)
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
