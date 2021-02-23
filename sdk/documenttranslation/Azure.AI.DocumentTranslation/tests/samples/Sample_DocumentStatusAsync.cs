// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.Samples
{
    [LiveOnly]
    public partial class DocumentTranslationSamples : SamplesBase<DocumentTranslationTestEnvironment>
    {
        [Test]
        public async Task DocumentStatusAsync()
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

            var operation = await client.StartBatchTranslationAsync(inputs);

            // get first document
            var documents = operation.GetAllDocumentsStatusAsync();
            IAsyncEnumerator<DocumentStatusDetail> docsEnumerator = documents.GetAsyncEnumerator();
            await docsEnumerator.MoveNextAsync();

            var doc = docsEnumerator.Current;

            // TODO: use string instead
            var docStatus = operation.GetDocumentStatus((Guid)doc.Id);

            while (docStatus.Value.Status != DocumentTranslationOperationStatus.Failed
                && docStatus.Value.Status != DocumentTranslationOperationStatus.Succeeded)
            {
                docStatus = operation.GetDocumentStatus((Guid)doc.Id);
            }

            Console.WriteLine($"Document {doc.Path} completed with status ${doc.Status}");
        }
    }
}
