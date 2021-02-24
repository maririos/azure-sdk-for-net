// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.AI.DocumentTranslation
{
    /// <summary> The DocumentTranslationOperation class for LRO. </summary>
    public class DocumentTranslationOperation : Operation<OperationStatusDetail>
    {
        /// <summary>Provides communication with the Translator Cognitive Service through its REST API.</summary>
        private readonly DocumentTranslationRestClient _serviceClient;

        /// <summary>Provides tools for exception creation in case of failure.</summary>
        private readonly ClientDiagnostics _diagnostics;

        /// <summary>
        /// Operation created date time.
        /// </summary>
        public DateTimeOffset CreatedOn => _createdOn;

        /// <summary>
        /// Date time in which the operation&apos;s status has been updated.
        /// </summary>
        public DateTimeOffset LastModified => _lastModified;

        /// <summary>
        /// List of possible statuses for job or document.
        /// </summary>
        public DocumentTranslationStatus Status => _status;

        /// <summary>
        /// Total number of documents in the operation.
        /// </summary>
        public int TotalDocuments => _totalDocuments;

        /// <summary>
        /// Number of documents failed to translate in the operation.
        /// </summary>
        public int DocumentsFailed => _documentsFailed;

        /// <summary>
        /// Number of documents translated successfully in the operation.
        /// </summary>
        public int DocumentsSucceeded => _documentsSucceeded;

        /// <summary>
        /// Number of documents in progress in the operation.
        /// </summary>
        public int DocumentsInProgress => _documentsInProgress;

        /// <summary>
        /// Number of documents in queue for translation.
        /// </summary>
        public int DocumentsNotStarted => _documentsNotStarted;

        /// <summary>
        /// Number of documents cancelled.
        /// </summary>
        public int DocumentsCancelled => _documentsCancelled;

        private int _totalDocuments;
        private int _documentsFailed;
        private int _documentsSucceeded;
        private int _documentsInProgress;
        private int _documentsNotStarted;
        private int _documentsCancelled;
        private DateTimeOffset _createdOn;
        private DateTimeOffset _lastModified;
        private DocumentTranslationStatus _status;

        /// <summary>
        /// Gets an ID representing the operation that can be used to poll for the status
        /// of the long-running operation.
        /// </summary>
        public override string Id { get; }

        /// <summary>
        /// Final result of the long-running operation.
        /// </summary>
        /// <remarks>
        /// This property can be accessed only after the operation completes successfully (HasValue is true).
        /// </remarks>
        public override OperationStatusDetail Value
        {
            get
            {
                if (HasCompleted && !HasValue)
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
                    throw _requestFailedException;
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
                else
                {
                    return OperationHelpers.GetValue(ref _value);
                }
            }
        }

        /// <summary><c>true</c> if the long-running operation has completed. Otherwise, <c>false</c>.</summary>
        private bool _hasCompleted;

        /// <summary>
        /// Returns true if the long-running operation completed.
        /// </summary>
        public override bool HasCompleted => _hasCompleted;

        private RequestFailedException _requestFailedException;

        /// <summary>The last HTTP response received from the server. <c>null</c> until the first response is received.</summary>
        private Response _response;

        /// <summary>The result of the long-running operation. <c>null</c> until result is received on status update.</summary>
        private OperationStatusDetail _value;

        /// <summary>
        /// Returns true if the long-running operation completed successfully and has produced final result (accessible by Value property).
        /// </summary>
        public override bool HasValue => _value != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTranslationOperation"/> class.
        /// </summary>
        /// <param name="operationId">The ID of this operation.</param>
        /// <param name="client">The client used to check for completion.</param>
        public DocumentTranslationOperation(string operationId, DocumentTranslationClient client)
        {
            // TODO: Add argument validation here.

            Id = operationId;
            _serviceClient = client._serviceRestClient;
            _diagnostics = client._clientDiagnostics;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTranslationOperation"/> class.
        /// </summary>
        /// <param name="serviceClient">The client for communicating with the Translator Cognitive Service through its REST API.</param>
        /// <param name="diagnostics">The client diagnostics for exception creation in case of failure.</param>
        /// <param name="operationLocation">The address of the long-running operation. It can be obtained from the response headers upon starting the operation.</param>
        internal DocumentTranslationOperation(DocumentTranslationRestClient serviceClient, ClientDiagnostics diagnostics, string operationLocation)
        {
            _serviceClient = serviceClient;
            _diagnostics = diagnostics;
            Id = operationLocation.Split('/').Last();
        }

        /// <summary>
        /// The last HTTP response received from the server.
        /// </summary>
        /// <remarks>
        /// The last response returned from the server during the lifecycle of this instance.
        /// An instance of <see cref="DocumentTranslationOperation"/> sends requests to a server in UpdateStatusAsync, UpdateStatus, and other methods.
        /// Responses from these requests can be accessed using GetRawResponse.
        /// </remarks>
        public override Response GetRawResponse() => _response;

        /// <summary>
        /// Calls the server to get updated status of the long-running operation.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used for the service call.</param>
        /// <returns>The HTTP response received from the server.</returns>
        /// <remarks>
        /// This operation will update the value returned from GetRawResponse and might update HasCompleted, HasValue, and Value.
        /// </remarks>
        public override Response UpdateStatus(CancellationToken cancellationToken = default) =>
            UpdateStatusAsync(false, cancellationToken).EnsureCompleted();

        /// <summary>
        /// Calls the server to get updated status of the long-running operation.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used for the service call.</param>
        /// <returns>The HTTP response received from the server.</returns>
        /// <remarks>
        /// This operation will update the value returned from GetRawResponse and might update HasCompleted, HasValue, and Value.
        /// </remarks>
        public override async ValueTask<Response> UpdateStatusAsync(CancellationToken cancellationToken = default) =>
            await UpdateStatusAsync(true, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Periodically calls the server till the long-running operation completes.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used for the periodical service calls.</param>
        /// <returns>The last HTTP response received from the server.</returns>
        /// <remarks>
        /// This method will periodically call UpdateStatusAsync till HasCompleted is true, then return the final result of the operation.
        /// </remarks>
        public override ValueTask<Response<OperationStatusDetail>> WaitForCompletionAsync(CancellationToken cancellationToken = default) =>
            this.DefaultWaitForCompletionAsync(cancellationToken);

        /// <summary>
        /// Periodically calls the server till the long-running operation completes.
        /// </summary>
        /// <param name="pollingInterval">
        /// The interval between status requests to the server.
        /// The interval can change based on information returned from the server.
        /// For example, the server might communicate to the client that there is not reason to poll for status change sooner than some time.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used for the periodical service calls.</param>
        /// <returns>The last HTTP response received from the server.</returns>
        /// <remarks>
        /// This method will periodically call UpdateStatusAsync till HasCompleted is true, then return the final result of the operation.
        /// </remarks>
        public override ValueTask<Response<OperationStatusDetail>> WaitForCompletionAsync(TimeSpan pollingInterval, CancellationToken cancellationToken = default) =>
            this.DefaultWaitForCompletionAsync(pollingInterval, cancellationToken);

        /// <summary>
        /// Calls the server to get updated status of the long-running operation.
        /// </summary>
        /// <param name="async">When <c>true</c>, the method will be executed asynchronously; otherwise, it will execute synchronously.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used for the service call.</param>
        /// <returns>The HTTP response received from the server.</returns>
        private async ValueTask<Response> UpdateStatusAsync(bool async, CancellationToken cancellationToken)
        {
            if (!_hasCompleted)
            {
                using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(UpdateStatus)}");
                scope.Start();

                try
                {
                    Response<OperationStatusDetail> update = async ?
                         await _serviceClient.GetOperationStatusAsync(new Guid(Id), cancellationToken).ConfigureAwait(false)
                        : _serviceClient.GetOperationStatus(new Guid(Id), cancellationToken);

                    _response = update.GetRawResponse();

                    _createdOn = update.Value.CreatedOn;
                    _lastModified = update.Value.LastModified;
                    _status = update.Value.Status;
                    _totalDocuments = update.Value.TotalDocuments;
                    _documentsFailed = update.Value.DocumentsFailed;
                    _documentsInProgress = update.Value.DocumentsInProgress;
                    _documentsSucceeded = update.Value.DocumentsSucceeded;
                    _documentsNotStarted = update.Value.DocumentsNotStarted;
                    _documentsCancelled = update.Value.DocumentsCancelled;

                    // TODO: Handle Failed and ValidationFailed correctly
                    if (update.Value.Status == DocumentTranslationStatus.Succeeded)
                    {
                        // we need to first assign a vaue and then mark the operation as completed to avoid race conditions
                        _value = update.Value;

                        _hasCompleted = true;
                    }
                    else if (update.Value.Status == DocumentTranslationStatus.Failed)
                    {
                        _requestFailedException = _diagnostics.CreateRequestFailedException(_response);
                        _hasCompleted = true;
                        throw _requestFailedException;
                    }
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            return GetRawResponse();
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
        public virtual async Task<Response<DocumentStatusDetail>> GetDocumentStatusAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(GetDocumentStatusAsync)}");
            scope.Start();

            try
            {
                return await _serviceClient.GetDocumentStatusAsync(new Guid(Id), documentId, cancellationToken).ConfigureAwait(false);
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

        /// <summary>
        /// Cancel the batch translation operation.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public virtual void Cancel(CancellationToken cancellationToken)
        {
            using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(Cancel)}");
            scope.Start();

            try
            {
                Response<OperationStatusDetail> response = _serviceClient.CancelOperation(new Guid(Id), cancellationToken);
                _response = response.GetRawResponse();
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Cancel the batch translation operation.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task CancelAsync(CancellationToken cancellationToken)
        {
            using DiagnosticScope scope = _diagnostics.CreateScope($"{nameof(DocumentTranslationOperation)}.{nameof(CancelAsync)}");
            scope.Start();

            try
            {
                Response<OperationStatusDetail> response = await _serviceClient.CancelOperationAsync(new Guid(Id), cancellationToken).ConfigureAwait(false);
                _response = response.GetRawResponse();
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
