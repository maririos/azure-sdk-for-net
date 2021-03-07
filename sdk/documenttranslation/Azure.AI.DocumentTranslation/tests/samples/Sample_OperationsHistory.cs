// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.Samples
{
    [LiveOnly]
    public partial class DocumentTranslationSamples : SamplesBase<DocumentTranslationTestEnvironment>
    {
        [Test]
        public void OperationsHistory()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            Pageable<TranslationStatusDetail> operationsStatus = client.GetSubmittedTranslations();

            int operationsCount = 0;
            int totalDocs = 0;
            int docsCancelled = 0;
            int docsSucceeded = 0;
            int maxDocs = 0;
            TranslationStatusDetail largestOperation = null;

            foreach (TranslationStatusDetail operationStatus in operationsStatus)
            {
                operationsCount++;
                totalDocs += operationStatus.DocumentsTotal;
                docsCancelled += operationStatus.DocumentsCancelled;
                docsSucceeded += operationStatus.DocumentsSucceeded;
                if (totalDocs > maxDocs)
                {
                    maxDocs = totalDocs;
                    largestOperation = operationStatus;
                }
            }

            Console.WriteLine($"# of operations: {operationsCount}");
            Console.WriteLine($"Total Documents: {totalDocs}");
            Console.WriteLine($"DocumentsSucceeded: {docsSucceeded}");
            Console.WriteLine($"Cancelled Documents: {docsCancelled}");

            Console.WriteLine($"Largest operation is {largestOperation.Id} and has the documents:");

            // After user studies we can do this if needed
            // DocumentTranslationOperation operation = largestOperation.GetOperation(client);
            DocumentTranslationOperation operation = new DocumentTranslationOperation(largestOperation.Id, client);

            Pageable<DocumentStatusDetail> docs = operation.GetDocumentsStatus();

            foreach (DocumentStatusDetail docStatus in docs)
            {
                Console.WriteLine($"Document {docStatus.Url} has status {docStatus.Status}");
            }
        }
    }
}
