// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.AI.DocumentTranslation.Models
{
    /// <summary>
    /// TranslationJobOptions
    /// </summary>
    public class TranslationJobOptions
    {
        /// <summary>
        /// SourceLanguage
        /// </summary>
        public string SourceLanguage { get; set; }

        /// <summary>
        /// Filter
        /// </summary>
        public DocumentFilter Filter { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// StorageType
        /// </summary>
        public StorageType? StorageType { get; set; }
    }
}
