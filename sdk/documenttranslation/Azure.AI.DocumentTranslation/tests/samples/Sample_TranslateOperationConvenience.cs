// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.samples
{
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

            var operation = client.StartBatchTranslation(new Uri(sourceUrl), "en",  new Uri(targetUrl), "it");

            TimeSpan pollingInterval = new TimeSpan(1000);

            while (!operation.HasCompleted)
            {
                Thread.Sleep(pollingInterval);
                operation.UpdateStatus();
            }

            Pageable<DocumentStatusDetail> response = operation.GetValues();

            var docsEnumerator = response.GetEnumerator();

            while (docsEnumerator.MoveNext())
            {
                if (docsEnumerator.Current.Status == DocumentTranslationOperationStatus.Succeeded)
                {
                    Console.WriteLine($"Document {docsEnumerator.Current.Path} succedded");
                }
                else
                {
                    Console.WriteLine($"Document {docsEnumerator.Current.Path} failed");
                }
            }
        }

    }
}
