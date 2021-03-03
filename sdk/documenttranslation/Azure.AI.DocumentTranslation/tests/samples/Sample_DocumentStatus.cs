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

            var inputs = new List<TranslationJobConfiguration>()
                {
                    new TranslationJobConfiguration(new SourceConfiguration(sourceUrl)
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

            Response<JobStatusDetail> job = client.CreateTranslationJob(inputs);

            // get not finished documents
            List<string> documentIds = new List<string>();
            Pageable<DocumentStatusDetail> documents = client.GetDocumentsStatus(job.Value.Id);

            foreach (DocumentStatusDetail docStatus in documents)
            {
                if (docStatus.Status ==TranslationStatus.NotStarted || docStatus.Status == TranslationStatus.Running)
                {
                    documentIds.Add(docStatus.Id);
                }
                else
                {
                    Console.WriteLine($"Document {docStatus.Url} completed with status ${docStatus.Status}");
                }
            }

            TimeSpan pollingInterval = new TimeSpan(1000);

            while (documentIds.Count > 0)
            {
                Thread.Sleep(pollingInterval);
                for (int i = documentIds.Count - 1; i >= 0; i--)
                {
                    Response<DocumentStatusDetail> status = client.GetDocumentStatus(job.Value.Id, documentIds[i]);
                    if (status.Value.Status != TranslationStatus.NotStarted && status.Value.Status != TranslationStatus.Running)
                    {
                        Console.WriteLine($"Document {status.Value.Url} completed with status ${status.Value.Status}");
                        documentIds.RemoveAt(i);
                    }
                }
            }
        }
    }
}
