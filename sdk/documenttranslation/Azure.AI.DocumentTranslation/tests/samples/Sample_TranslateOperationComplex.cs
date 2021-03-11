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
            Uri sourceUrl1 = new Uri(TestEnvironment.SourceBlobContainerSas);
            Uri sourceUrl2 = new Uri(TestEnvironment.SourceBlobContainerSas);
            Uri targetUrl1 = new Uri(TestEnvironment.TargetBlobContainerSas);
            Uri targetUrl2 = new Uri(TestEnvironment.TargetBlobContainerSas);
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

            DocumentTranslationOperation operation = client.StartTranslation(inputs);

            TimeSpan pollingInterval = new TimeSpan(1000);

            while (!operation.HasCompleted)
            {
                Thread.Sleep(pollingInterval);
                operation.UpdateStatus();

                Console.WriteLine($"  Status: {operation.Status}");
                Console.WriteLine($"  Created on: {operation.CreatedOn}");
                Console.WriteLine($"  Last modified: {operation.LastModified}");
                Console.WriteLine($"  Total documents: {operation.DocumentsTotal}");
                Console.WriteLine($"    Succeeded: {operation.DocumentsSucceeded}");
                Console.WriteLine($"    Failed: {operation.DocumentsFailed}");
                Console.WriteLine($"    In Progress: {operation.DocumentsInProgress}");
                Console.WriteLine($"    Not started: {operation.DocumentsNotStarted}");
            }

            foreach (DocumentStatusDetail document in operation.GetValues())
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
