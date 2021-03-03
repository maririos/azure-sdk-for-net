// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
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

            // get first document
            Pageable<DocumentStatusDetail> documents = operation.GetDocumentsStatus();
            IEnumerator<DocumentStatusDetail> docsEnumerator = documents.GetEnumerator();
            docsEnumerator.MoveNext();

            DocumentStatusDetail doc = docsEnumerator.Current;

            Response<DocumentStatusDetail> docStatus = operation.GetDocumentStatus(doc.Id);

            while (docStatus.Value.Status != DocumentTranslationStatus.Failed
                && docStatus.Value.Status != DocumentTranslationStatus.Succeeded)
            {
                docStatus = operation.GetDocumentStatus(doc.Id);
            }

            Console.WriteLine($"Document {doc.Url} completed with status ${doc.Status}");
        }
    }
}
