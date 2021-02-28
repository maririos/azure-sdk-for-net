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
        public void DocumentStatus()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;
            string sourceUrl = TestEnvironment.SourceUrl;
            string targetUrl = TestEnvironment.TargetUrl;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var inputs = new List<BatchDocumentInput>()
                {
                    new BatchDocumentInput(new SourceInput(sourceUrl)
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

            Response<string> operation = client.StartBatchTranslation(inputs);

            // get first document
            Pageable<DocumentStatusDetail> documents = client.GetStatusesOfDocuments(operation.Value);
            IEnumerator<DocumentStatusDetail> docsEnumerator = documents.GetEnumerator();
            docsEnumerator.MoveNext();

            DocumentStatusDetail doc = docsEnumerator.Current;

            TimeSpan pollingInterval = new TimeSpan(1000);

            Response<DocumentStatusDetail> docStatus = client.GetDocumentStatus(operation.Value, doc.Id);

            while (docStatus.Value.Status != DocumentTranslationStatus.Failed
                && docStatus.Value.Status != DocumentTranslationStatus.Succeeded)
            {
                Thread.Sleep(pollingInterval);
                docStatus = client.GetDocumentStatus(operation.Value, doc.Id);
            }

            Console.WriteLine($"Document {doc.Url} completed with status ${doc.Status}");
        }
    }
}
