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

            var input1 = new TranslationOperationConfiguration(
                new SourceConfiguration(sourceUrl),
                new List<TargetConfiguration>()
                    {
                        new TargetConfiguration(targetUrl, "it", new List<TranslationGlossary> {new TranslationGlossary(glossaryUrl)})
                    },
                StorageType.Folder);

            var input2 = new TranslationOperationConfiguration(
                new SourceConfiguration(targetUrl),
                new List<TargetConfiguration>()
                    {
                        new TargetConfiguration(sourceUrl, "en", new List<TranslationGlossary> {new TranslationGlossary(glossaryUrl)})
                    },
                StorageType.Folder);

            var inputs = new List<TranslationOperationConfiguration>()
                {
                    input1,
                    input2
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
                Console.WriteLine($"  Total documents: {operation.TotalDocuments}");
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
