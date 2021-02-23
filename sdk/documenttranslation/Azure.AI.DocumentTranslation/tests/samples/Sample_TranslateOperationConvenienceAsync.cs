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

            DocumentTranslationOperation operation = await client.StartBatchTranslationAsync(new Uri(sourceUrl), "es", new Uri(targetUrl), "en");

            await operation.WaitForCompletionAsync();

            Console.WriteLine("Operation Information:");
            Console.WriteLine($"  Id: {operation.Id}");
            Console.WriteLine($"  Status: {operation.Status}");
            Console.WriteLine($"  Created on: {operation.CreatedOn}");
            Console.WriteLine($"  Last modified: {operation.LastModified}");
            Console.WriteLine($"  Total documents: {operation.TotalDocuments}");
            Console.WriteLine($"    Succeeded: {operation.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {operation.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {operation.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {operation.DocumentsNotStarted}");

            await foreach (DocumentStatusDetail document in operation.Value)
            {
                Console.WriteLine($"Document with Id: {document.Id}");
                Console.WriteLine($"  Status:{document.Status}");
                if (document.Status == DocumentTranslationOperationStatus.Succeeded)
                {
                    Console.WriteLine($"  Location: {document.Path}");
                    Console.WriteLine($"  Translated to language: {document.To}.");
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
