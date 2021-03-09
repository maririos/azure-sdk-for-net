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
            string sourceUrl1 = TestEnvironment.SourceUrl;
            string sourceUrl2 = TestEnvironment.SourceUrl;
            string targetUrl1 = TestEnvironment.TargetUrl;
            string targetUrl2 = TestEnvironment.TargetUrl;
            Uri glossaryUrl = new Uri(TestEnvironment.GlossaryUrl);

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var glossaries = new List<TranslationGlossary>() { new TranslationGlossary(glossaryUrl) };

            var configuration1 = new TranslationConfiguration(
                source: new TranslationSource(sourceUrl1),
                targets: new List<TranslationTarget>() { new TranslationTarget(targetUrl1, "it", glossaries) },
                storageType: StorageType.Folder);

            var configuration2 = new TranslationConfiguration(
                source: new TranslationSource(sourceUrl2),
                targets: new List<TranslationTarget>() { new TranslationTarget(targetUrl2, "it", glossaries) },
                storageType: StorageType.Folder);

            var inputs = new List<TranslationConfiguration>()
                {
                    configuration1,
                    configuration2
                };

            Response<JobStatusDetail> jobStatus = client.CreateTranslationJob(inputs);

            TimeSpan pollingInterval = new TimeSpan(1000);

            while (jobStatus.Value.Status != TranslationStatus.Failed
                       || jobStatus.Value.Status != TranslationStatus.Succeeded
                       || jobStatus.Value.Status != TranslationStatus.ValidationFailed)
            {
                Thread.Sleep(pollingInterval);
                jobStatus = client.GetJobStatus(jobStatus.Value.Id);
            }
            Console.WriteLine($"  Status: {jobStatus.Value.Status}");
            Console.WriteLine($"  Created on: {jobStatus.Value.CreatedOn}");
            Console.WriteLine($"  Last modified: {jobStatus.Value.LastModified}");
            Console.WriteLine($"  Total documents: {jobStatus.Value.DocumentsTotal}");
            Console.WriteLine($"    Succeeded: {jobStatus.Value.DocumentsSucceeded}");
            Console.WriteLine($"    Failed: {jobStatus.Value.DocumentsFailed}");
            Console.WriteLine($"    In Progress: {jobStatus.Value.DocumentsInProgress}");
            Console.WriteLine($"    Not started: {jobStatus.Value.DocumentsNotStarted}");

            // Get Status of documents
            Pageable<DocumentStatusDetail> documents = client.GetDocumentsStatus(jobStatus.Value.Id);

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
