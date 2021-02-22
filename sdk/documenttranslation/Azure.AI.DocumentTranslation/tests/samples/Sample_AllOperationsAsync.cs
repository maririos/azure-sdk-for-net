// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.samples
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
            while (await operationsEnumerator.MoveNextAsync())
            {
                Console.WriteLine(JsonSerializer.Serialize(operationsEnumerator.Current, new JsonSerializerOptions { WriteIndented = true }));
            }
        }
    }
}
