// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.ResourceManager.MachineLearningServices.Models
{
    /// <summary> The RGitHubPackage. </summary>
    public partial class RGitHubPackage
    {
        /// <summary> Initializes a new instance of RGitHubPackage. </summary>
        public RGitHubPackage()
        {
        }

        /// <summary> Repository address in the format username/repo[/subdir][@ref|#pull]. </summary>
        public string Repository { get; set; }
        /// <summary> Personal access token to install from a private repo. </summary>
        public string AuthToken { get; set; }
    }
}
