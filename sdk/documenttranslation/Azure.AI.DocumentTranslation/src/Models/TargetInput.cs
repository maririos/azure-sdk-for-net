// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;

namespace Azure.AI.DocumentTranslation.Models
{
    [CodeGenModel("TargetInput")]
    public partial class TargetInput
    {
        [CodeGenMember("StorageSource")]
        internal string StorageSource { get; set;}
    }
}
