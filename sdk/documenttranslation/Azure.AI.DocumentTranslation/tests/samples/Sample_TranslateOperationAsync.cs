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

            DocumentTranslationOperation operation = await client.StartBatchTranslationAsync(inputs);

            Response<OperationStatusDetail> response = await operation.WaitForCompletionAsync();

            // response has the same fields in the operation class

            Console.WriteLine($"  Status: {response.Value.Status} \t {operation.Status}");
            Console.WriteLine($"  Created on: {response.Value.CreatedOn} \t {operation.CreatedOn}");
            Console.WriteLine($"  Last modified: {response.Value.LastModified} \t {operation.LastModified}");
            Console.WriteLine($"  Total documents: {response.Value.TotalDocuments} \t {operation.TotalDocuments}");
            Console.WriteLine($"    Succeeded: {response.Value.DocumentsSucceeded} \t {operation.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {response.Value.DocumentsFailed} \t {operation.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {response.Value.DocumentsInProgress} \t {operation.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {response.Value.DocumentsNotStarted} \t {operation.DocumentsNotStarted}");

            // Get Status of documents
            AsyncPageable<DocumentStatusDetail> documents = client.GetStatusesOfDocumentsAsync(operation.Id);

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
