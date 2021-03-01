// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
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

            Pageable<JobStatusDetail> jobs = client.GetStatusesOfJobs();

            int jobsCount = 0;
            int totalDocs = 0;
            int docsCancelled = 0;
            int docsSucceeded = 0;
            int maxDocs = 0;
            string largestJobId = "";

            foreach (JobStatusDetail job in jobs)
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
            Pageable<DocumentStatusDetail> docs = client.GetStatusesOfDocuments(largestJobId);

            foreach (DocumentStatusDetail docStatus in docs)
            {
                Console.WriteLine($"Document {docStatus.Url} has status {docStatus.Status}");
            }
        }
    }
}
