// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.ResourceManager.Sql.Models;

namespace Azure.ResourceManager.Sql
{
    /// <summary> The RestorePoints service client. </summary>
    public partial class RestorePointsOperations
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal RestorePointsRestOperations RestClient { get; }
        /// <summary> Initializes a new instance of RestorePointsOperations for mocking. </summary>
        protected RestorePointsOperations()
        {
        }
        /// <summary> Initializes a new instance of RestorePointsOperations. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="subscriptionId"> The subscription ID that identifies an Azure subscription. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal RestorePointsOperations(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, string subscriptionId, Uri endpoint = null)
        {
            RestClient = new RestorePointsRestOperations(clientDiagnostics, pipeline, subscriptionId, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Gets a restore point. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="restorePointName"> The name of the restore point. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<RestorePoint>> GetAsync(string resourceGroupName, string serverName, string databaseName, string restorePointName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.Get");
            scope.Start();
            try
            {
                return await RestClient.GetAsync(resourceGroupName, serverName, databaseName, restorePointName, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Gets a restore point. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="restorePointName"> The name of the restore point. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<RestorePoint> Get(string resourceGroupName, string serverName, string databaseName, string restorePointName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.Get");
            scope.Start();
            try
            {
                return RestClient.Get(resourceGroupName, serverName, databaseName, restorePointName, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Deletes a restore point. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="restorePointName"> The name of the restore point. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteAsync(string resourceGroupName, string serverName, string databaseName, string restorePointName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.Delete");
            scope.Start();
            try
            {
                return await RestClient.DeleteAsync(resourceGroupName, serverName, databaseName, restorePointName, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Deletes a restore point. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="restorePointName"> The name of the restore point. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Delete(string resourceGroupName, string serverName, string databaseName, string restorePointName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.Delete");
            scope.Start();
            try
            {
                return RestClient.Delete(resourceGroupName, serverName, databaseName, restorePointName, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Gets a list of database restore points. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="resourceGroupName"/>, <paramref name="serverName"/>, or <paramref name="databaseName"/> is null. </exception>
        public virtual AsyncPageable<RestorePoint> ListByDatabaseAsync(string resourceGroupName, string serverName, string databaseName, CancellationToken cancellationToken = default)
        {
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (serverName == null)
            {
                throw new ArgumentNullException(nameof(serverName));
            }
            if (databaseName == null)
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            async Task<Page<RestorePoint>> FirstPageFunc(int? pageSizeHint)
            {
                using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.ListByDatabase");
                scope.Start();
                try
                {
                    var response = await RestClient.ListByDatabaseAsync(resourceGroupName, serverName, databaseName, cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value, null, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            return PageableHelpers.CreateAsyncEnumerable(FirstPageFunc, null);
        }

        /// <summary> Gets a list of database restore points. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="resourceGroupName"/>, <paramref name="serverName"/>, or <paramref name="databaseName"/> is null. </exception>
        public virtual Pageable<RestorePoint> ListByDatabase(string resourceGroupName, string serverName, string databaseName, CancellationToken cancellationToken = default)
        {
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (serverName == null)
            {
                throw new ArgumentNullException(nameof(serverName));
            }
            if (databaseName == null)
            {
                throw new ArgumentNullException(nameof(databaseName));
            }

            Page<RestorePoint> FirstPageFunc(int? pageSizeHint)
            {
                using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.ListByDatabase");
                scope.Start();
                try
                {
                    var response = RestClient.ListByDatabase(resourceGroupName, serverName, databaseName, cancellationToken);
                    return Page.FromValues(response.Value.Value, null, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            return PageableHelpers.CreateEnumerable(FirstPageFunc, null);
        }

        /// <summary> Creates a restore point for a data warehouse. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="parameters"> The definition for creating the restore point of this database. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="resourceGroupName"/>, <paramref name="serverName"/>, <paramref name="databaseName"/>, or <paramref name="parameters"/> is null. </exception>
        public virtual async Task<RestorePointsCreateOperation> StartCreateAsync(string resourceGroupName, string serverName, string databaseName, CreateDatabaseRestorePointDefinition parameters, CancellationToken cancellationToken = default)
        {
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (serverName == null)
            {
                throw new ArgumentNullException(nameof(serverName));
            }
            if (databaseName == null)
            {
                throw new ArgumentNullException(nameof(databaseName));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.StartCreate");
            scope.Start();
            try
            {
                var originalResponse = await RestClient.CreateAsync(resourceGroupName, serverName, databaseName, parameters, cancellationToken).ConfigureAwait(false);
                return new RestorePointsCreateOperation(_clientDiagnostics, _pipeline, RestClient.CreateCreateRequest(resourceGroupName, serverName, databaseName, parameters).Request, originalResponse);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Creates a restore point for a data warehouse. </summary>
        /// <param name="resourceGroupName"> The name of the resource group that contains the resource. You can obtain this value from the Azure Resource Manager API or the portal. </param>
        /// <param name="serverName"> The name of the server. </param>
        /// <param name="databaseName"> The name of the database. </param>
        /// <param name="parameters"> The definition for creating the restore point of this database. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="resourceGroupName"/>, <paramref name="serverName"/>, <paramref name="databaseName"/>, or <paramref name="parameters"/> is null. </exception>
        public virtual RestorePointsCreateOperation StartCreate(string resourceGroupName, string serverName, string databaseName, CreateDatabaseRestorePointDefinition parameters, CancellationToken cancellationToken = default)
        {
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (serverName == null)
            {
                throw new ArgumentNullException(nameof(serverName));
            }
            if (databaseName == null)
            {
                throw new ArgumentNullException(nameof(databaseName));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var scope = _clientDiagnostics.CreateScope("RestorePointsOperations.StartCreate");
            scope.Start();
            try
            {
                var originalResponse = RestClient.Create(resourceGroupName, serverName, databaseName, parameters, cancellationToken);
                return new RestorePointsCreateOperation(_clientDiagnostics, _pipeline, RestClient.CreateCreateRequest(resourceGroupName, serverName, databaseName, parameters).Request, originalResponse);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}