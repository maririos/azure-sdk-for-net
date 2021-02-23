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
        public void AllOperations()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var operations = client.GetAllOperations();
            var operationsEnumerator = operations.GetEnumerator();
            operationsEnumerator.MoveNext();

            var latestOperation = operationsEnumerator.Current;
            var operation = new DocumentTranslationOperation(latestOperation.Id.ToString(), client);

            var documents = operation.GetAllDocumentsStatus();
            IEnumerator<DocumentStatusDetail> docsEnumerator = documents.GetEnumerator();

            while (docsEnumerator.MoveNext())
            {
                Console.WriteLine($"Document {docsEnumerator.Current.Path} has status {docsEnumerator.Current.Status}");
            }
        }
    }
}
