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
            Uri sourceUrl = new Uri(TestEnvironment.SourceUrl);
            Uri targetUrl = new Uri(TestEnvironment.TargetUrl);

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            DocumentTranslationOperation operation = client.StartTranslation(sourceUrl, targetUrl, "it");

            var documentscompleted = new HashSet<string>();

            while (!operation.HasCompleted)
            {
                operation.UpdateStatus();

                Pageable<DocumentStatusDetail> documentsStatus = operation.GetDocumentsStatus();
                foreach (DocumentStatusDetail docStatus in documentsStatus)
                {
                    if (documentscompleted.Contains(docStatus.Id))
                        continue;
                    if (docStatus.Status == TranslationStatus.Succeeded || docStatus.Status == TranslationStatus.Failed)
                    {
                        documentscompleted.Add(docStatus.Id);
                        Console.WriteLine($"Document {docStatus.Url} completed with status ${docStatus.Status}");
                    }
                }
            }
        }
    }
}
