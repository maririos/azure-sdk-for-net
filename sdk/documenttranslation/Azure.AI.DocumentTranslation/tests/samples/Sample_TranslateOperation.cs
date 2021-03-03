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
        public void TranslateOperation()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            string sourceUrl = TestEnvironment.SourceUrl;
            string targetUrl = TestEnvironment.TargetUrl;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var inputs = new List<TranlsationOperationConfiguration>()
                {
                    new TranlsationOperationConfiguration(new SourceConfiguration(sourceUrl)
                        {
                            Language = "en"
                        },
                    new List<TargetConfiguration>()
                        {
                            new TargetConfiguration(targetUrl, "it")
                        })
                    {
                        StorageType = StorageInputType.Folder
                    }
                };

            DocumentTranslationOperation operation = client.StartTranslation(inputs);

            TimeSpan pollingInterval = new TimeSpan(1000);

            while (!operation.HasCompleted)
            {
                Thread.Sleep(pollingInterval);
                operation.UpdateStatus();
            }

            Pageable<DocumentStatusDetail> response = operation.GetValues();
            IEnumerator<DocumentStatusDetail> docsEnumerator = response.GetEnumerator();

            while (docsEnumerator.MoveNext())
            {
                if (docsEnumerator.Current.Status == DocumentTranslationStatus.Succeeded)
                {
                    Console.WriteLine($"Document {docsEnumerator.Current.Url} succedded");
                }
                else
                {
                    Console.WriteLine($"Document {docsEnumerator.Current.Url} failed");
                }
            }
        }
    }
}
