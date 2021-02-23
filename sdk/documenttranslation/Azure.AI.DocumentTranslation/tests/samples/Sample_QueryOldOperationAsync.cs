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
    public partial class DocumentTranslationSamples : SamplesBase<DocumentTranslationTestEnvironment>
    {
        [Test]
        public async Task AllOperationsAsync()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var operations = client.GetAllOperationsAsync();
            var operationsEnumerator = operations.GetAsyncEnumerator();
            await operationsEnumerator.MoveNextAsync();

            var latestOperation = operationsEnumerator.Current;
            var operation = new DocumentTranslationOperation(latestOperation.Id.ToString(), client);

            var documents = operation.GetAllDocumentsStatusAsync();
            IAsyncEnumerator<DocumentStatusDetail> docsEnumerator = documents.GetAsyncEnumerator();

            while (await docsEnumerator.MoveNextAsync())
            {
                Console.WriteLine($"Document {docsEnumerator.Current.Path} has status {docsEnumerator.Current.Status}");
            }
        }
    }
}
