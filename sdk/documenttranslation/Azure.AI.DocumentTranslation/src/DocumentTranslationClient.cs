// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.DocumentTranslation.Models;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.AI.DocumentTranslation
{
    /// <summary>
    /// The client to use for interacting with the Azure Translator Service.
    /// </summary>
    public class DocumentTranslationClient
    {
        internal readonly DocumentTranslationRestClient _serviceRestClient;
        internal readonly ClientDiagnostics _clientDiagnostics;
        internal readonly DocumentTranslationClientOptions _options;

        private const string AuthorizationHeader = "Ocp-Apim-Subscription-Key";
        private readonly string DefaultCognitiveScope = "https://cognitiveservices.azure.com/.default";

        //TODO: Configure polling interval
        private readonly TimeSpan _pollinInterval = new TimeSpan(1000);

        /// <summary>
        /// Protected constructor to allow mocking.
        /// </summary>
        protected DocumentTranslationClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentTranslationClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">A <see cref="TokenCredential"/> used to
        /// authenticate requests to the service, such as DefaultAzureCredential.</param>
        /// <param name="options"><see cref="DocumentTranslationClientOptions"/> that allow
        /// callers to configure how requests are sent to the service.</param>
        public DocumentTranslationClient(Uri endpoint, TokenCredential credential, DocumentTranslationClientOptions options)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            Argument.AssertNotNull(credential, nameof(credential));
            Argument.AssertNotNull(options, nameof(options));

            _options = options;
            _clientDiagnostics = new ClientDiagnostics(options);

            var pipeline = HttpPipelineBuilder.Build(options, new BearerTokenAuthenticationPolicy(credential, DefaultCognitiveScope));
            _serviceRestClient = new DocumentTranslationRestClient(_clientDiagnostics, pipeline, endpoint.AbsoluteUri);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentTranslationClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">A <see cref="TokenCredential"/> used to
        /// authenticate requests to the service, such as DefaultAzureCredential.</param>
        public DocumentTranslationClient(Uri endpoint, TokenCredential credential)
            : this(endpoint, credential, new DocumentTranslationClientOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentTranslationClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">The API key used to access
        /// the service. This will allow you to update the API key
        /// without creating a new client.</param>
        /// <param name="options"><see cref="DocumentTranslationClientOptions"/> that allow
        /// callers to configure how requests are sent to the service.</param>
        public DocumentTranslationClient(Uri endpoint, AzureKeyCredential credential, DocumentTranslationClientOptions options)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            Argument.AssertNotNull(credential, nameof(credential));
            Argument.AssertNotNull(options, nameof(options));

            _options = options;
            _clientDiagnostics = new ClientDiagnostics(options);

            var pipeline = HttpPipelineBuilder.Build(options, new AzureKeyCredentialPolicy(credential, AuthorizationHeader));
            _serviceRestClient = new DocumentTranslationRestClient(_clientDiagnostics, pipeline, endpoint.AbsoluteUri);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentTranslationClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">The API key used to access
        /// the service. This will allow you to update the API key
        /// without creating a new client.</param>
        public DocumentTranslationClient(Uri endpoint, AzureKeyCredential credential)
            : this(endpoint, credential, new DocumentTranslationClientOptions())
        {
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="configurations"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Response<JobStatusDetail> CreateTranslationJob(List<TranslationJobConfiguration> configurations, CancellationToken cancellationToken = default)
        {
            var request = new BatchSubmissionRequest(configurations);
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(CreateTranslationJob)}");
            scope.Start();

            try
            {
                ResponseWithHeaders<DocumentTranslationSubmitBatchRequestHeaders> response = _serviceRestClient.SubmitBatchRequest(request, cancellationToken);
                string id = response.Headers.OperationLocation;
                return _serviceRestClient.GetOperationStatus(new Guid(id), cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="configurations"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Response<JobStatusDetail>> CreateTranslationJobAsync(List<TranslationJobConfiguration> configurations, CancellationToken cancellationToken = default)
        {
            var request = new BatchSubmissionRequest(configurations);
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(CreateTranslationJobAsync)}");
            scope.Start();

            try
            {
                ResponseWithHeaders<DocumentTranslationSubmitBatchRequestHeaders> response = await _serviceRestClient.SubmitBatchRequestAsync(request, cancellationToken).ConfigureAwait(false);
                string id = response.Headers.OperationLocation;
                return await _serviceRestClient.GetOperationStatusAsync(new Guid(id), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="glossaries"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Response<JobStatusDetail> CreateTranslationJob(Uri sourceUrl, Uri targetUrl, string targetLanguage, List<TranslationGlossary> glossaries = default, TranslationOperationOptions options = default, CancellationToken cancellationToken = default)
        {
            var source = new SourceConfiguration(sourceUrl.AbsoluteUri)
            {
                Language = options.SourceLanguage,
                Filter = options.Filter
            };

            var targets = new List<TargetConfiguration>
            {
                new TargetConfiguration(targetUrl.AbsoluteUri, targetLanguage, glossaries)
                {
                    Category = options.Category
                }
            };
            var request = new BatchSubmissionRequest(new List<TranslationJobConfiguration>
                {
                    new TranslationJobConfiguration(source, targets)
                    {
                        StorageType = options.StorageType
                    }
                });

            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(CreateTranslationJob)}");
            scope.Start();

            try
            {
                ResponseWithHeaders<DocumentTranslationSubmitBatchRequestHeaders> job = _serviceRestClient.SubmitBatchRequest(request, cancellationToken);
                var id = job.Headers.OperationLocation.Split('/').Last();
                return _serviceRestClient.GetOperationStatus(new Guid(id), cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="glossaries"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Response<JobStatusDetail>> CreateTranslationJobAsync(Uri sourceUrl, Uri targetUrl, string targetLanguage, List<TranslationGlossary> glossaries = default, TranslationOperationOptions options = default, CancellationToken cancellationToken = default)
        {
            var source = new SourceConfiguration(sourceUrl.AbsoluteUri)
            {
                Language = options.SourceLanguage,
                Filter = options.Filter
            };

            var targets = new List<TargetConfiguration>
            {
                new TargetConfiguration(targetUrl.AbsoluteUri, targetLanguage, glossaries)
                {
                    Category = options.Category
                }
            };
            var request = new BatchSubmissionRequest(new List<TranslationJobConfiguration>
                {
                    new TranslationJobConfiguration(source, targets)
                    {
                        StorageType = options.StorageType
                    }
                });

            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(CreateTranslationJobAsync)}");
            scope.Start();

            try
            {
                ResponseWithHeaders<DocumentTranslationSubmitBatchRequestHeaders> job = await _serviceRestClient.SubmitBatchRequestAsync(request, cancellationToken).ConfigureAwait(false);
                var id = job.Headers.OperationLocation.Split('/').Last();
                return await _serviceRestClient.GetOperationStatusAsync(new Guid(id), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Calls the server to get status of the translation job.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used for the service call.</param>
        /// <returns></returns>
        public virtual async Task<Response<JobStatusDetail>> GetJobStatusAsync(string jobId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetJobStatusAsync)}");
            scope.Start();

            try
            {
                return await _serviceRestClient.GetOperationStatusAsync(new Guid(jobId), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Calls the server to get status of the translation job.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used for the service call.</param>
        /// <returns></returns>
        public virtual Response<JobStatusDetail> GetJobStatus(string jobId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetJobStatus)}");
            scope.Start();

            try
            {
                return _serviceRestClient.GetOperationStatus(new Guid(jobId), cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Response<JobStatusDetail>> WaitForJobCompletionAsync(string jobId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(WaitForJobCompletionAsync)}");
            scope.Start();
            Response<JobStatusDetail> status;

            try
            {
                do
                {
                    await Task.Delay(_pollinInterval, cancellationToken).ConfigureAwait(false);
                    status = await _serviceRestClient.GetOperationStatusAsync(new Guid(jobId), cancellationToken).ConfigureAwait(false);
                }
                while (status.Value.Status != TranslationStatus.Failed
                       || status.Value.Status != TranslationStatus.Succeeded
                       || status.Value.Status != TranslationStatus.ValidationFailed);
                return status;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Response<JobStatusDetail> WaitForJobCompletion(string jobId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(WaitForJobCompletionAsync)}");
            scope.Start();
            Response<JobStatusDetail> status;

            try
            {
                do
                {
                    Thread.Sleep(_pollinInterval);
                    status = _serviceRestClient.GetOperationStatus(new Guid(jobId), cancellationToken);
                }
                while (status.Value.Status != TranslationStatus.Failed
                       || status.Value.Status != TranslationStatus.Succeeded
                       || status.Value.Status != TranslationStatus.ValidationFailed);
                return status;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Pageable<JobStatusDetail> GetJobsStatus(CancellationToken cancellationToken = default)
        {
            Page<JobStatusDetail> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetJobsStatus)}");
                scope.Start();

                try
                {
                    var response = _serviceRestClient.GetOperations(cancellationToken: cancellationToken);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            Page<JobStatusDetail> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetJobsStatus)}");
                scope.Start();

                try
                {
                    Response<BatchStatusResponse> response = _serviceRestClient.GetOperationsNextPage(nextLink, cancellationToken: cancellationToken);
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
        /// a.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual AsyncPageable<JobStatusDetail> GetJobsStatusAsync(CancellationToken cancellationToken = default)
        {
            async Task<Page<JobStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetJobsStatusAsync)}");
                scope.Start();

                try
                {
                    var response = await _serviceRestClient.GetOperationsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            async Task<Page<JobStatusDetail>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetJobsStatusAsync)}");
                scope.Start();

                try
                {
                    Response<BatchStatusResponse> response = await _serviceRestClient.GetOperationsNextPageAsync(nextLink, cancellationToken: cancellationToken).ConfigureAwait(false);
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
        /// Get the status of a specific document in the batch.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="documentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Response<DocumentStatusDetail> GetDocumentStatus(string jobId, string documentId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetDocumentStatus)}");
            scope.Start();

            try
            {
                return _serviceRestClient.GetDocumentStatus(new Guid(jobId), new Guid(documentId), cancellationToken);
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
        /// <param name="jobId"></param>
        /// <param name="documentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Response<DocumentStatusDetail>> GetDocumentStatusAsync(string jobId, string documentId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetDocumentStatusAsync)}");
            scope.Start();

            try
            {
                return await _serviceRestClient.GetDocumentStatusAsync(new Guid(jobId), new Guid(documentId), cancellationToken).ConfigureAwait(false);
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
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Pageable<DocumentStatusDetail> GetDocumentsStatus(string jobId, CancellationToken cancellationToken = default)
        {
            Page<DocumentStatusDetail> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetDocumentsStatus)}");
                scope.Start();

                try
                {
                    var response = _serviceRestClient.GetOperationDocumentsStatus(new Guid(jobId), null, null, cancellationToken);
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
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetDocumentsStatus)}");
                scope.Start();

                try
                {
                    var response = _serviceRestClient.GetOperationDocumentsStatusNextPage(nextLink, new Guid(jobId), cancellationToken: cancellationToken);
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
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual AsyncPageable<DocumentStatusDetail> GetDocumentsStatusAsync(string jobId, CancellationToken cancellationToken = default)
        {
            async Task<Page<DocumentStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetDocumentsStatusAsync)}");
                scope.Start();

                try
                {
                    var response = await _serviceRestClient.GetOperationDocumentsStatusAsync(new Guid(jobId), null, null, cancellationToken).ConfigureAwait(false);
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
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetDocumentsStatusAsync)}");
                scope.Start();

                try
                {
                    var response = await _serviceRestClient.GetOperationDocumentsStatusNextPageAsync(nextLink, new Guid(jobId), cancellationToken: cancellationToken).ConfigureAwait(false);
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
        /// a.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        public virtual Response<JobStatusDetail> CancelTranslationJob(string jobId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(CancelTranslationJob)}");
            scope.Start();

            try
            {
                return _serviceRestClient.CancelOperation(new Guid(jobId), cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Response<JobStatusDetail>> CancelTranslationJobAsync(string jobId, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(CancelTranslationJobAsync)}");
            scope.Start();

            try
            {
                return await _serviceRestClient.CancelOperationAsync(new Guid(jobId), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        #region supported formats functions

        internal virtual Response<IReadOnlyList<FileFormat>> GetSupportedGlossaryFormats(CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetSupportedGlossaryFormats)}");
            scope.Start();

            try
            {
                var response = _serviceRestClient.GetGlossaryFormats(cancellationToken);
                return Response.FromValue(response.Value.Value, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal virtual async Task<Response<IReadOnlyList<FileFormat>>> GetSupportedGlossaryFormatsAsync(CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetSupportedGlossaryFormatsAsync)}");
            scope.Start();

            try
            {
                var response = await _serviceRestClient.GetGlossaryFormatsAsync(cancellationToken).ConfigureAwait(false);
                return Response.FromValue(response.Value.Value, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal virtual Response<IReadOnlyList<FileFormat>> GetSupportedDocumentFormats(CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetSupportedDocumentFormats)}");
            scope.Start();

            try
            {
                var response = _serviceRestClient.GetDocumentFormats(cancellationToken);
                return Response.FromValue(response.Value.Value, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal virtual async Task<Response<IReadOnlyList<FileFormat>>> GetSupportedDocumentFormatsAsync(CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetSupportedDocumentFormatsAsync)}");
            scope.Start();

            try
            {
                var response = await _serviceRestClient.GetDocumentFormatsAsync(cancellationToken).ConfigureAwait(false);
                return Response.FromValue(response.Value.Value, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal virtual Response<IReadOnlyList<StorageSource>> GetSupportedStorageSources(CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetSupportedStorageSources)}");
            scope.Start();

            try
            {
                var response = _serviceRestClient.GetDocumentStorageSource(cancellationToken);
                return Response.FromValue(response.Value.Value, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal virtual async Task<Response<IReadOnlyList<StorageSource>>> GetSupportedStorageSourcesAsync(CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetSupportedStorageSourcesAsync)}");
            scope.Start();

            try
            {
                var response = await _serviceRestClient.GetDocumentStorageSourceAsync(cancellationToken).ConfigureAwait(false);
                return Response.FromValue(response.Value.Value, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        #endregion

        #region nobody wants to see these
        /// <summary>
        /// Check if two TextAnalyticsClient instances are equal.
        /// </summary>
        /// <param name="obj">The instance to compare to.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => base.Equals(obj);

        /// <summary>
        /// Get a hash code for the TextAnalyticsClient.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// TextAnalyticsClient ToString.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() => base.ToString();
        #endregion
    }
}
