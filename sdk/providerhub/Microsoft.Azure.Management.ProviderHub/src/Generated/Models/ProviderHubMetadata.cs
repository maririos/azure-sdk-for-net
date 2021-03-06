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
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ProviderHubMetadata
    {
        /// <summary>
        /// Initializes a new instance of the ProviderHubMetadata class.
        /// </summary>
        public ProviderHubMetadata()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ProviderHubMetadata class.
        /// </summary>
        public ProviderHubMetadata(IList<ResourceProviderAuthorization> providerAuthorizations = default(IList<ResourceProviderAuthorization>), ProviderHubMetadataProviderAuthentication providerAuthentication = default(ProviderHubMetadataProviderAuthentication), ProviderHubMetadataThirdPartyProviderAuthorization thirdPartyProviderAuthorization = default(ProviderHubMetadataThirdPartyProviderAuthorization))
        {
            ProviderAuthorizations = providerAuthorizations;
            ProviderAuthentication = providerAuthentication;
            ThirdPartyProviderAuthorization = thirdPartyProviderAuthorization;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "providerAuthorizations")]
        public IList<ResourceProviderAuthorization> ProviderAuthorizations { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "providerAuthentication")]
        public ProviderHubMetadataProviderAuthentication ProviderAuthentication { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "thirdPartyProviderAuthorization")]
        public ProviderHubMetadataThirdPartyProviderAuthorization ThirdPartyProviderAuthorization { get; set; }

    }
}
