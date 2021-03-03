// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public virtual DocumentTranslationOperation StartTranslation(List<TranlsationOperationConfiguration> configurations, CancellationToken cancellationToken = default)
        {
            var request = new BatchSubmissionRequest(configurations);
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(StartTranslation)}");
            scope.Start();

            try
            {
                var job = _serviceRestClient.SubmitBatchRequest(request, cancellationToken);
                return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
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
        public virtual async Task<DocumentTranslationOperation> StartTranslationAsync(List<TranlsationOperationConfiguration> configurations, CancellationToken cancellationToken = default)
        {
            var request = new BatchSubmissionRequest(configurations);
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(StartTranslationAsync)}");
            scope.Start();

            try
            {
                var job = await _serviceRestClient.SubmitBatchRequestAsync(request, cancellationToken).ConfigureAwait(false);
                return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
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
        /// <param name="sourceLanguage"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="glossaries"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal virtual DocumentTranslationOperation StartTranslation(Uri sourceUrl, string sourceLanguage, Uri targetUrl, string targetLanguage, List<TranslationGlossary> glossaries, CancellationToken cancellationToken = default)
        {
            var request = new BatchSubmissionRequest(new List<TranlsationOperationConfiguration>
                {
                    new TranlsationOperationConfiguration(new SourceConfiguration(sourceUrl.AbsoluteUri) { Language = sourceLanguage }, new List<TargetConfiguration>
                        {
                            new TargetConfiguration(targetUrl.AbsoluteUri, targetLanguage, glossaries)
                        })
                });

            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(StartTranslation)}");
            scope.Start();

            try
            {
                var job = _serviceRestClient.SubmitBatchRequest(request, cancellationToken);
                return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
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
        /// <param name="sourceLanguage"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="glossaries"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal virtual async Task<DocumentTranslationOperation> StartTranslationAsync(Uri sourceUrl, string sourceLanguage, Uri targetUrl, string targetLanguage, List<TranslationGlossary> glossaries, CancellationToken cancellationToken = default)
        {
            var request = new BatchSubmissionRequest(new List<TranlsationOperationConfiguration>
                {
                    new TranlsationOperationConfiguration(new SourceConfiguration(sourceUrl.AbsoluteUri) { Language = sourceLanguage }, new List<TargetConfiguration>
                        {
                            new TargetConfiguration(targetUrl.AbsoluteUri, targetLanguage, glossaries)
                        })
                });

            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(StartTranslationAsync)}");
            scope.Start();

            try
            {
                var job = await _serviceRestClient.SubmitBatchRequestAsync(request, cancellationToken).ConfigureAwait(false);
                return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
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
        public virtual Pageable<OperationStatusDetail> GetOperationsStatus(CancellationToken cancellationToken = default)
        {
            Page<OperationStatusDetail> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetOperationsStatus)}");
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

            Page<OperationStatusDetail> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetOperationsStatus)}");
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
        public virtual AsyncPageable<OperationStatusDetail> GetOperationsStatusAsync(CancellationToken cancellationToken = default)
        {
            async Task<Page<OperationStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetOperationsStatusAsync)}");
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

            async Task<Page<OperationStatusDetail>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetOperationsStatusAsync)}");
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
