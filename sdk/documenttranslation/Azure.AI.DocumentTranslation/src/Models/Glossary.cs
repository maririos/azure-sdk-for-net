// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;

namespace Azure.AI.DocumentTranslation.Models
{
    [CodeGenModel("Glossary")]
    public partial class TranslationGlossary
    {
        /// <summary> Storage Source. </summary>
        [CodeGenMember("StorageSource")]
        internal string StorageSource { get; set; }

        /// <summary> Format. </summary>
        [CodeGenMember("Format")]
        public string FormatVersion { get; set; }
    }
}
