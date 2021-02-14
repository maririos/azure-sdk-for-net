// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;

namespace Azure.AI.DocumentTranslation
{
    public interface IDocumentTranslationClient
    {
        Pageable<BatchStatusDetail> GetBatchOperations(CancellationToken cancellationToken = default);
        AsyncPageable<BatchStatusDetail> GetBatchOperationsAsync(CancellationToken cancellationToken = default);
        Pageable<DocumentStatusDetail> GetOperationDocumentsStatus(string jobId, CancellationToken cancellationToken = default);
        AsyncPageable<DocumentStatusDetail> GetOperationDocumentsStatusAsync(string jobId, CancellationToken cancellationToken = default);
        DocumentTranslationOperation StartBatchTranslation(BatchSubmissionRequest request, CancellationToken cancellationToken = default);
        DocumentTranslationOperation StartBatchTranslation(Uri sourceUrl, string sourceLanguage, Uri targetUrl, string targetLanguage, CancellationToken cancellationToken = default);
        Task<DocumentTranslationOperation> StartBatchTranslationAsync(BatchSubmissionRequest request, CancellationToken cancellationToken = default);
        Task<DocumentTranslationOperation> StartBatchTranslationAsync(Uri sourceUrl, string sourceLanguage, Uri targetUrl, string targetLanguage, CancellationToken cancellationToken = default);
    }
}