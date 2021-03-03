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

            var inputs = new List<TranslationOperationConfiguration>()
                {
                    new TranslationOperationConfiguration(new SourceConfiguration(sourceUrl)
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

            var documentsSucceeded = new HashSet<string>();
            var documentsFailed = new HashSet<string>();

            while (!operation.HasCompleted)
            {
                operation.UpdateStatus();

                // update list when any document is done
                if (operation.DocumentsSucceeded > documentsSucceeded.Count || operation.DocumentsFailed > documentsFailed.Count)
                {
                    Pageable<DocumentStatusDetail> documentsStatus = operation.GetDocumentsStatus();
                    foreach (DocumentStatusDetail docStatus in documentsStatus)
                    {
                        if (docStatus.Status == TranslationStatus.Succeeded)
                        {
                            documentsSucceeded.Add(docStatus.Id);
                        }
                        else if (docStatus.Status == TranslationStatus.Failed)
                        {
                            documentsFailed.Add(docStatus.Id);
                        }
                    }
                }
            }

            Console.WriteLine($"Documents Succeeded: {documentsSucceeded}");
            Console.WriteLine($"Documents Failed: {documentsFailed}");
        }
    }
}
