// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core;

namespace Azure.AI.DocumentTranslation.Models
{
    [CodeGenModel("DocumentStatusDetail")]
    public partial class DocumentStatusDetail
    {
        /// <summary> Document Id. </summary>
        [CodeGenMember("Id")]
        public string Id { get; }
        /// <summary> Location of the document or folder. </summary>
        [CodeGenMember("Path")]
#pragma warning disable CA1056 // URI-like properties should not be strings
        public string Url { get; }
#pragma warning restore CA1056 // URI-like properties should not be strings

        /// <summary> Progress of the translation if available. </summary>
        [CodeGenMember("Progress")]
        public float TranslationProgress { get; }

        /// <summary> To language. </summary>
        [CodeGenMember("To")]
        public string TranslateTo { get; }

        /// <summary> Document created date time. </summary>
        [CodeGenMember("CreatedDateTimeUtc")]
        public DateTimeOffset CreatedOn { get; }

        /// <summary> Date time in which the Document's status has been updated. </summary>
        [CodeGenMember("LastActionDateTimeUtc")]
        public DateTimeOffset LastModified { get; }
    }
}
