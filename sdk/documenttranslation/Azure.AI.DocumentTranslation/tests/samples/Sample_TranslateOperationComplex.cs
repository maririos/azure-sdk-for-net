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
        public void TranslateOperationComplex()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            string sourceUrl = TestEnvironment.SourceUrl;
            string targetUrl = TestEnvironment.TargetUrl;
            Uri glossaryUrl = new Uri(TestEnvironment.GlossaryUrl);

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var input1 = new TranslationJobConfiguration(
                new SourceConfiguration(sourceUrl),
                new List<TargetConfiguration>()
                    {
                        new TargetConfiguration(targetUrl, "it", new List<TranslationGlossary> {new TranslationGlossary(glossaryUrl)})
                    },
                StorageType.Folder);

            var input2 = new TranslationJobConfiguration(
                new SourceConfiguration(targetUrl),
                new List<TargetConfiguration>()
                    {
                        new TargetConfiguration(sourceUrl, "en", new List<TranslationGlossary> {new TranslationGlossary(glossaryUrl)})
                    },
                StorageType.Folder);

            var inputs = new List<TranslationJobConfiguration>()
                {
                    input1,
                    input2
                };

            Response<JobStatusDetail> job = client.CreateTranslationJob(inputs);

            Response<JobStatusDetail> jobStatus = client.WaitForJobCompletion(job.Value.Id);

            Console.WriteLine($"  Status: {jobStatus.Value.Status}");
            Console.WriteLine($"  Created on: {jobStatus.Value.CreatedOn}");
            Console.WriteLine($"  Last modified: {jobStatus.Value.LastModified}");
            Console.WriteLine($"  Total documents: {jobStatus.Value.TotalDocuments}");
            Console.WriteLine($"    Succeeded: {jobStatus.Value.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {jobStatus.Value.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {jobStatus.Value.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {jobStatus.Value.DocumentsNotStarted}");

            // Get Status of documents
            Pageable<DocumentStatusDetail> documents = client.GetDocumentsStatus(job.Value.Id);

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
