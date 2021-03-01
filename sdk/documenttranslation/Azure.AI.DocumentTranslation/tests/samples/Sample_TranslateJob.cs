// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
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
            string sourceUrl = TestEnvironment.SourceUrl;
            string targetUrl = TestEnvironment.TargetUrl;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var inputs = new List<DocumentTranslationInput>()
                {
                    new DocumentTranslationInput(new SourceInput(sourceUrl)
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

            Response<JobStatusDetail> job = client.CreateTranslationJob(inputs);

            var jobStatus = client.WaitForJobCompletion(job.Value.Id);

            Console.WriteLine($"  Status: {jobStatus.Value.Status}");
            Console.WriteLine($"  Created on: {jobStatus.Value.CreatedOn}");
            Console.WriteLine($"  Last modified: {jobStatus.Value.LastModified}");
            Console.WriteLine($"  Total documents: {jobStatus.Value.TotalDocuments}");
            Console.WriteLine($"    Succeeded: {jobStatus.Value.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {jobStatus.Value.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {jobStatus.Value.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {jobStatus.Value.DocumentsNotStarted}");

            // Get Status of documents
            Pageable<DocumentStatusDetail> documents = client.GetStatusesOfDocuments(job.Value.Id);

            foreach (DocumentStatusDetail document in documents)
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
