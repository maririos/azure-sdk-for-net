// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.Samples
{
    [LiveOnly]
    public partial class DocumentTranslationSamples : SamplesBase<DocumentTranslationTestEnvironment>
    {
        [Test]
        public void TranslateOperationConvenience()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            string sourceUrl = TestEnvironment.SourceUrl;
            string targetUrl = TestEnvironment.TargetUrl;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            Response<string> operation = client.StartBatchTranslation(new Uri(sourceUrl), "en",  new Uri(targetUrl), "it");

            Console.WriteLine("Operation Information:");
            Console.WriteLine($"  Id: {operation.Value}");

            Response<OperationStatusDetail> status = client.GetOperationStatus(operation.Value);

            TimeSpan pollingInterval = new TimeSpan(1000);
            while (status.Value.Status != DocumentTranslationStatus.Failed
                   || status.Value.Status != DocumentTranslationStatus.Succeeded
                   || status.Value.Status != DocumentTranslationStatus.ValidationFailed)
            {
                Thread.Sleep(pollingInterval);
                status = client.GetOperationStatus(operation.Value);
            }

            Console.WriteLine($"  Status: {status.Value.Status}");
            Console.WriteLine($"  Created on: {status.Value.CreatedOn}");
            Console.WriteLine($"  Last modified: {status.Value.LastModified}");
            Console.WriteLine($"  Total documents: {status.Value.TotalDocuments}");
            Console.WriteLine($"    Succeeded: {status.Value.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {status.Value.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {status.Value.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {status.Value.DocumentsNotStarted}");

            // Get Status of documents
            Pageable<DocumentStatusDetail> documents = client.GetStatusesOfDocuments(operation.Value);

            foreach (DocumentStatusDetail document in documents)
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
