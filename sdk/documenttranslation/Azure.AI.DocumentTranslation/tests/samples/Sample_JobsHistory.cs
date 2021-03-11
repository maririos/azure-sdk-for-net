// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
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
        public void JobsHistory()
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

            foreach (JobStatusDetail job in client.GetSubmittedJobs())
            {
                if (!job.HasCompleted)
                {
                    Thread.Sleep(pollingInterval);
                    client.GetJobStatus(job.Id);
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
