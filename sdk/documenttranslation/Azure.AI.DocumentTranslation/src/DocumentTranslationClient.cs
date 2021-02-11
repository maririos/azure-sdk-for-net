// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
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
        internal readonly TranslationRestClient _serviceRestClient;
        internal readonly ClientDiagnostics _clientDiagnostics;
        internal readonly TranslatorClientOptions _options;

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
        /// <param name="options"><see cref="TranslatorClientOptions"/> that allow
        /// callers to configure how requests are sent to the service.</param>
        public DocumentTranslationClient(Uri endpoint, TokenCredential credential, TranslatorClientOptions options)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            Argument.AssertNotNull(credential, nameof(credential));
            Argument.AssertNotNull(options, nameof(options));

            _options = options;
            _clientDiagnostics = new ClientDiagnostics(options);

            var pipeline = HttpPipelineBuilder.Build(options, new BearerTokenAuthenticationPolicy(credential, DefaultCognitiveScope));
            _serviceRestClient = new TranslationRestClient(_clientDiagnostics, pipeline, endpoint.AbsoluteUri);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentTranslationClient"/>
        /// </summary>
        /// <param name="endpoint">A <see cref="Uri"/> to the service the client
        /// sends requests to.  Endpoint can be found in the Azure portal.</param>
        /// <param name="credential">A <see cref="TokenCredential"/> used to
        /// authenticate requests to the service, such as DefaultAzureCredential.</param>
        public DocumentTranslationClient(Uri endpoint, TokenCredential credential)
            : this(endpoint, credential, new TranslatorClientOptions())
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
        /// <param name="options"><see cref="TranslatorClientOptions"/> that allow
        /// callers to configure how requests are sent to the service.</param>
        public DocumentTranslationClient(Uri endpoint, AzureKeyCredential credential, TranslatorClientOptions options)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            Argument.AssertNotNull(credential, nameof(credential));
            Argument.AssertNotNull(options, nameof(options));

            _options = options;
            _clientDiagnostics = new ClientDiagnostics(options);

            var pipeline = HttpPipelineBuilder.Build(options, new AzureKeyCredentialPolicy(credential, AuthorizationHeader));
            _serviceRestClient = new TranslationRestClient(_clientDiagnostics, pipeline, endpoint.AbsoluteUri);
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
            : this(endpoint, credential, new TranslatorClientOptions())
        {
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual DocumentTranslationOperation StartBatchTranslation(BatchSubmissionRequest request, CancellationToken cancellationToken = default)
        {
            var job = _serviceRestClient.SubmitBatchRequest(request, cancellationToken);
            return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<DocumentTranslationOperation> StartBatchTranslationAsync(BatchSubmissionRequest request, CancellationToken cancellationToken = default)
        {
            var job = await _serviceRestClient.SubmitBatchRequestAsync(request, cancellationToken).ConfigureAwait(false);
            return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual DocumentTranslationOperation StartBatchTranslation(Uri sourceUrl, string sourceLanguage, Uri targetUrl, string targetLanguage, CancellationToken cancellationToken = default)
        {
            // TODO: remove sourceLanguage when service supports automatic language detection
            var request = new BatchSubmissionRequest(new List<BatchRequest>
                {
                    new BatchRequest(new SourceInput(sourceUrl.AbsoluteUri) { Language = sourceLanguage }, new List<TargetInput>
                        {
                            new TargetInput(targetUrl.AbsoluteUri, targetLanguage)
                        })
                });
            var job = _serviceRestClient.SubmitBatchRequest(request, cancellationToken);
            return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<DocumentTranslationOperation> StartBatchTranslationAsync(Uri sourceUrl, string sourceLanguage, Uri targetUrl, string targetLanguage, CancellationToken cancellationToken = default)
        {
            // TODO: remove sourceLanguage when service supports automatic language detection
            var request = new BatchSubmissionRequest(new List<BatchRequest>
                {
                    new BatchRequest(new SourceInput(sourceUrl.AbsoluteUri) { Language = sourceLanguage }, new List<TargetInput>
                        {
                            new TargetInput(targetUrl.AbsoluteUri, targetLanguage)
                        })
                });
            var job = await _serviceRestClient.SubmitBatchRequestAsync(request, cancellationToken).ConfigureAwait(false);
            return new DocumentTranslationOperation(_serviceRestClient, _clientDiagnostics, job.Headers.OperationLocation);
        }

        /// <summary>
        /// a.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public AsyncPageable<BatchStatusDetail> GetBatchOperations(CancellationToken cancellationToken = default)
        {
            async Task<Page<BatchStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetBatchOperations)}");
                scope.Start();

                try
                {
                    var response = await _serviceRestClient.GetOperationsAsync(null, null, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            async Task<Page<BatchStatusDetail>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetBatchOperations)}");
                scope.Start();

                try
                {
                    int top = default;
                    int skip = default;

                    // Extracting parameters from the URL.
                    // nextLink - https://westus.api.cognitive.microsoft.com/translator/text/batch/v1.0-preview.1/batches?$skip=20&$top=0

                    string @params = nextLink.Split('?').Last();
                    // params = '$skip=20&$top=0'

                    // Extracting Top and Skip parameter values
                    string[] parameters = @params.Split('&');
                    // '$skip=20', '$top=0'

                    foreach (string paramater in parameters)
                    {
                        if (paramater.Contains("top"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out top);
                            // 0
                        }
                        if (paramater.Contains("skip"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out skip);
                            // 20
                        }
                    }

                    Response<BatchStatusResponse> response = await _serviceRestClient.GetOperationsAsync(top, skip, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
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
        /// <returns></returns>
        public AsyncPageable<DocumentStatusDetail> GetOperationDocumentsStatus(string jobId, CancellationToken cancellationToken = default)
        {
            async Task<Page<DocumentStatusDetail>> FirstPageFunc(int? pageSizeHint)
            {
                pageSizeHint = 3;
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetOperationDocumentsStatus)}");
                scope.Start();

                try
                {
                    var response = await _serviceRestClient.GetOperationDocumentsStatusAsync(new Guid(jobId), null, null, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }

            async Task<Page<DocumentStatusDetail>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                pageSizeHint = 3;
                using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(DocumentTranslationClient)}.{nameof(GetBatchOperations)}");
                scope.Start();

                try
                {
                    int top = default;
                    int skip = default;

                    // Extracting parameters from the URL.
                    // nextLink - https://westus.api.cognitive.microsoft.com/translator/text/batch/v1.0-preview.1/batches/8002878d-2e43-4675-ad20-455fe004641b/documents?$skip=20&$top=0

                    string @params = nextLink.Split('?').Last();
                    // params = '$skip=20&$top=0'

                    // Extracting Top and Skip parameter values
                    string[] parameters = @params.Split('&');
                    // '$skip=20', '$top=0'

                    foreach (string paramater in parameters)
                    {
                        if (paramater.Contains("top"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out top);
                            // 0
                        }
                        if (paramater.Contains("skip"))
                        {
                            _ = int.TryParse(paramater.Split('=')[1], out skip);
                            // 20
                        }
                    }

                    Response<DocumentStatusResponse> response = await _serviceRestClient.GetOperationDocumentsStatusAsync(new Guid(jobId), top, skip, cancellationToken).ConfigureAwait(false);

                    var result = response.Value;
                    return Page.FromValues(result.Value, result.NextLink, response.GetRawResponse());
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
