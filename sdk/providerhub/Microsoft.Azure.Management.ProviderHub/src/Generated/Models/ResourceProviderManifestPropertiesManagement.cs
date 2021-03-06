// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.ProviderHub.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ResourceProviderManifestPropertiesManagement : ResourceProviderManagement
    {
        /// <summary>
        /// Initializes a new instance of the
        /// ResourceProviderManifestPropertiesManagement class.
        /// </summary>
        public ResourceProviderManifestPropertiesManagement()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// ResourceProviderManifestPropertiesManagement class.
        /// </summary>
        /// <param name="resourceAccessPolicy">Possible values include:
        /// 'NotSpecified', 'AcisReadAllowed', 'AcisActionAllowed'</param>
        public ResourceProviderManifestPropertiesManagement(IList<string> schemaOwners = default(IList<string>), IList<string> manifestOwners = default(IList<string>), string incidentRoutingService = default(string), string incidentRoutingTeam = default(string), string incidentContactEmail = default(string), IList<ServiceTreeInfo> serviceTreeInfos = default(IList<ServiceTreeInfo>), string resourceAccessPolicy = default(string), IList<object> resourceAccessRoles = default(IList<object>))
            : base(schemaOwners, manifestOwners, incidentRoutingService, incidentRoutingTeam, incidentContactEmail, serviceTreeInfos, resourceAccessPolicy, resourceAccessRoles)
        {
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

    }
}
