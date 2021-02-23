// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Azure.AI.DocumentTranslation.Models
{
    /// <summary> The DocumentStatusDetail. </summary>
    public partial class DocumentStatusDetail
    {
        /// <summary> Initializes a new instance of DocumentStatusDetail. </summary>
        /// <param name="url"> Location of the document or folder. </param>
        /// <param name="createdOn"> Operation created date time. </param>
        /// <param name="lastModified"> Date time in which the operation&apos;s status has been updated. </param>
        /// <param name="status"> List of possible statuses for job or document. </param>
        /// <param name="translateTo"> To language. </param>
        /// <param name="translationProgress"> Progress of the translation if available. </param>
        /// <param name="id"> Document Id. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="url"/> or <paramref name="translateTo"/> is null. </exception>
        internal DocumentStatusDetail(string url, DateTimeOffset createdOn, DateTimeOffset lastModified, DocumentTranslationStatus status, string translateTo, float translationProgress, Guid id)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (translateTo == null)
            {
                throw new ArgumentNullException(nameof(translateTo));
            }

            Url = url;
            CreatedOn = createdOn;
            LastModified = lastModified;
            Status = status;
            TranslateTo = translateTo;
            TranslationProgress = translationProgress;
            Id = id;
        }

        /// <summary> Initializes a new instance of DocumentStatusDetail. </summary>
        /// <param name="url"> Location of the document or folder. </param>
        /// <param name="createdOn"> Operation created date time. </param>
        /// <param name="lastModified"> Date time in which the operation&apos;s status has been updated. </param>
        /// <param name="status"> List of possible statuses for job or document. </param>
        /// <param name="translateTo"> To language. </param>
        /// <param name="error"> This contains an outer error with error code, message, details, target and an inner error with more descriptive details. </param>
        /// <param name="translationProgress"> Progress of the translation if available. </param>
        /// <param name="id"> Document Id. </param>
        /// <param name="characterCharged"> Character charged by the API. </param>
        internal DocumentStatusDetail(string url, DateTimeOffset createdOn, DateTimeOffset lastModified, DocumentTranslationStatus status, string translateTo, ErrorV2 error, float translationProgress, Guid id, long? characterCharged)
        {
            Url = url;
            CreatedOn = createdOn;
            LastModified = lastModified;
            Status = status;
            TranslateTo = translateTo;
            Error = error;
            TranslationProgress = translationProgress;
            Id = id;
            CharacterCharged = characterCharged;
        }
        /// <summary> List of possible statuses for job or document. </summary>
        public DocumentTranslationStatus Status { get; }
        /// <summary> This contains an outer error with error code, message, details, target and an inner error with more descriptive details. </summary>
        public ErrorV2 Error { get; }
        /// <summary> Document Id. </summary>
        public Guid Id { get; }
        /// <summary> Character charged by the API. </summary>
        public long? CharacterCharged { get; }
    }
}
