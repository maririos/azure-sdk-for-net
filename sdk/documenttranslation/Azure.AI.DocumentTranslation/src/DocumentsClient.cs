// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.AI.DocumentTranslation
{
    /// <summary>
    /// The client to use for getting documents statuses from the service.
    /// </summary>
    public class DocumentsClient
    {
        internal readonly DocumentTranslationRestClient _serviceClient;
        internal readonly ClientDiagnostics _diagnostics;

        /// <summary>
        /// Gets an ID representing the operation that can be used to poll for the status
        /// of the long-running operation.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Protected paramterless constructor to allow mocking.
        /// </summary>
        protected DocumentsClient()
        {
        }

        internal DocumentsClient(DocumentTranslationClient client, string operationId)
        {
            _serviceClient = client._serviceRestClient;
            _diagnostics = client._clientDiagnostics;
            Id = operationId;
        }

        /// <summary>
        /// Get the status of a specific document in the batch.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Response<DocumentStatusDetail> GetDocumentStatus(string documentId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(GetDocumentStatus)}");
            scope.Start();

            try
            {
                return _serviceClient.GetDocumentStatus(new Guid(Id), new Guid(documentId), cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Get the status of a specific document in the batch.
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Response<DocumentStatusDetail>> GetDocumentStatusAsync(string documentId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(GetDocumentStatusAsync)}");
            scope.Start();

            try
            {
                return await _serviceClient.GetDocumentStatusAsync(new Guid(Id), new Guid(documentId), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Get the status of a all documents in the batch.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Pageable<DocumentStatusDetail> GetStatusesOfDocuments(CancellationToken cancellationToken = default)
        {
            Page<DocumentStatusDetail> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(GetStatusesOfDocuments)}");
                scope.Start();

                try
                {
                    var response = _serviceClient.GetOperationDocumentsStatus(new Guid(Id), null, null, cancellationToken);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            Page<DocumentStatusDetail> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(GetStatusesOfDocuments)}");
                scope.Start();

                try
                {
                    var response = _serviceClient.GetOperationDocumentsStatusNextPage(nextLink, new Guid(Id), cancellationToken: cancellationToken);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            return PageableHelpers.CreateEnumerable(FirstPageFunc, NextPageFunc);
        }

        /// <summary>
        /// Get the status of a all documents in the batch.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual AsyncPageable<DocumentStatusDetail> GetStatusesOfDocumentsAsync(CancellationToken cancellationToken = default)
        {
            async Task<Page<DocumentStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(GetStatusesOfDocumentsAsync)}");
                scope.Start();

                try
                {
                    var response = await _serviceClient.GetOperationDocumentsStatusAsync(new Guid(Id), null, null, cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            async Task<Page<DocumentStatusDetail>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(GetStatusesOfDocumentsAsync)}");
                scope.Start();

                try
                {
                    var response = await _serviceClient.GetOperationDocumentsStatusNextPageAsync(nextLink, new Guid(Id), cancellationToken: cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            return PageableHelpers.CreateAsyncEnumerable(FirstPageFunc, NextPageFunc);
        }
    }
}
