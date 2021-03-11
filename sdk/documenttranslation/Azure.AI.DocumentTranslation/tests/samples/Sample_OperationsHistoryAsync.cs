// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.Samples
{
    public partial class DocumentTranslationSamples : SamplesBase<DocumentTranslationTestEnvironment>
    {
        [Test]
        public async Task OperationsHistoryAsync()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            int operationsCount = 0;
            int totalDocs = 0;
            int docsCancelled = 0;
            int docsSucceeded = 0;
            int docsFailed = 0;

            await foreach (TranslationStatusDetail translationStatus in client.GetSubmittedTranslationsAsync())
            {
                if (!translationStatus.HasCompleted)
                {
                    // After user studies we can do this if needed
                    // DocumentTranslationOperation operation = translationStatus.GetOperation(client);
                    DocumentTranslationOperation operation = new (translationStatus.Id, client);
                    await operation.WaitForCompletionAsync();
                }

                operationsCount++;
                totalDocs += translationStatus.DocumentsTotal;
                docsCancelled += translationStatus.DocumentsCancelled;
                docsSucceeded += translationStatus.DocumentsSucceeded;
                docsFailed += translationStatus.DocumentsFailed;
            }

            Console.WriteLine($"# of operations: {operationsCount}");
            Console.WriteLine($"Total Documents: {totalDocs}");
            Console.WriteLine($"Succeeded Document: {docsSucceeded}");
            Console.WriteLine($"Failed Document: {docsFailed}");
            Console.WriteLine($"Cancelled Documents: {docsCancelled}");
        }
    }
}
