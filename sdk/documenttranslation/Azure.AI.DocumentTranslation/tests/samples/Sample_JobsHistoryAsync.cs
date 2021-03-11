// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.DocumentTranslation.Tests.Samples
{
    public partial class DocumentTranslationSamples : SamplesBase<DocumentTranslationTestEnvironment>
    {
        [Test]
        public async Task JobsHistoryAsync()
        {
            string endpoint = TestEnvironment.Endpoint;
            string apiKey = TestEnvironment.ApiKey;

            var client = new DocumentTranslationClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            TimeSpan pollingInterval = new TimeSpan(1000);

            int jobsCount = 0;
            int docsTotal = 0;
            int docsCancelled = 0;
            int docsSucceeded = 0;
            int docsFailed = 0;

            await foreach (JobStatusDetail job in client.GetSubmittedJobsAsync())
            {
                if (!job.HasCompleted)
                {
                    await Task.Delay(pollingInterval);
                    await client.GetJobStatusAsync(job.Id);
                }

                jobsCount++;
                docsTotal += job.DocumentsTotal;
                docsCancelled += job.DocumentsCancelled;
                docsSucceeded += job.DocumentsSucceeded;
                docsFailed += job.DocumentsFailed;
            }

            Console.WriteLine($"# of jobs: {jobsCount}");
            Console.WriteLine($"Total Documents: {docsTotal}");
            Console.WriteLine($"Succeeded Document: {docsSucceeded}");
            Console.WriteLine($"Failed Document: {docsFailed}");
            Console.WriteLine($"Cancelled Documents: {docsCancelled}");
        }
    }
}
