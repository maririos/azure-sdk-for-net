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
        public void TranslateJob()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            Uri sourceUrl = new Uri(TestEnvironment.SourceBlobContainerSas);
            Uri targetUrl = new Uri(TestEnvironment.TargetBlobContainerSas);

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            JobStatusDetail jobStatus = client.CreateTranslationJobForAzureBlobs(sourceUrl, targetUrl, "it");

            TimeSpan pollingInterval = new TimeSpan(1000);

            while (!jobStatus.HasCompleted)
            {
                Thread.Sleep(pollingInterval);
                jobStatus = client.GetJobStatus(jobStatus.Id);
            }

            Console.WriteLine($"  Status: {jobStatus.Status}");
            Console.WriteLine($"  Created on: {jobStatus.CreatedOn}");
            Console.WriteLine($"  Last modified: {jobStatus.LastModified}");
            Console.WriteLine($"  Total documents: {jobStatus.DocumentsTotal}");
            Console.WriteLine($"    Succeeded: {jobStatus.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {jobStatus.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {jobStatus.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {jobStatus.DocumentsNotStarted}");

            foreach (DocumentStatusDetail document in client.GetDocumentsStatus(jobStatus.Id))
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
