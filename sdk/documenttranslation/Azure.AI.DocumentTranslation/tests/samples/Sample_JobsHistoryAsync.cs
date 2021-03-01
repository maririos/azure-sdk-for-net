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

            AsyncPageable<JobStatusDetail> jobs = client.GetStatusesOfJobsAsync();

            int jobsCount = 0;
            int totalDocs = 0;
            int docsCancelled = 0;
            int docsSucceeded = 0;
            int maxDocs = 0;
            string largestJobId = "";

            await foreach (JobStatusDetail job in jobs)
            {
                jobsCount++;
                totalDocs += job.TotalDocuments;
                docsCancelled += job.DocumentsCancelled;
                docsSucceeded += job.DocumentsSucceeded;
                if (totalDocs > maxDocs)
                {
                    maxDocs = totalDocs;
                    largestJobId = job.Id;
                }
            }

            Console.WriteLine($"# of jobs: {jobsCount}\nTotal Documents: {totalDocs}\n"
                              + $"DocumentsSucceeded: {docsSucceeded}\n"
                              + $"Cancelled Documents: {docsCancelled}");

            Console.WriteLine($"Largest job is {largestJobId} and has the documents:");
            AsyncPageable<DocumentStatusDetail> docs = client.GetStatusesOfDocumentsAsync(largestJobId);

            await foreach (DocumentStatusDetail docStatus in docs)
            {
                Console.WriteLine($"Document {docStatus.Url} has status {docStatus.Status}");
            }
        }
    }
}
