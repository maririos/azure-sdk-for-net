// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Azure.AI.DocumentTranslation.Models
{
    /// <summary> Job status response. </summary>
    public partial class BatchStatusDetail
    {
        /// <summary> Initializes a new instance of BatchStatusDetail. </summary>
        /// <param name="id"> Id of the operation. </param>
        /// <param name="createdDateTimeUtc"> Operation created date time. </param>
        /// <param name="lastActionDateTimeUtc"> Date time in which the operation&apos;s status has been updated. </param>
        /// <param name="summary"> . </param>
        /// <exception cref="ArgumentNullException"> <paramref name="summary"/> is null. </exception>
        internal BatchStatusDetail(Guid id, DateTimeOffset createdDateTimeUtc, DateTimeOffset lastActionDateTimeUtc, StatusSummary summary)
        {
            if (summary == null)
            {
                throw new ArgumentNullException(nameof(summary));
            }

            Id = id;
            CreatedDateTimeUtc = createdDateTimeUtc;
            LastActionDateTimeUtc = lastActionDateTimeUtc;
            Summary = summary;
        }

        /// <summary> Initializes a new instance of BatchStatusDetail. </summary>
        /// <param name="id"> Id of the operation. </param>
        /// <param name="createdDateTimeUtc"> Operation created date time. </param>
        /// <param name="lastActionDateTimeUtc"> Date time in which the operation&apos;s status has been updated. </param>
        /// <param name="status"> List of possible statuses for job or document. </param>
        /// <param name="summary"> . </param>
        internal BatchStatusDetail(Guid id, DateTimeOffset createdDateTimeUtc, DateTimeOffset lastActionDateTimeUtc, Status? status, StatusSummary summary)
        {
            Id = id;
            CreatedDateTimeUtc = createdDateTimeUtc;
            LastActionDateTimeUtc = lastActionDateTimeUtc;
            Status = status;
            Summary = summary;
        }

        /// <summary> Id of the operation. </summary>
        public Guid Id { get; }
        /// <summary> Operation created date time. </summary>
        public DateTimeOffset CreatedDateTimeUtc { get; }
        /// <summary> Date time in which the operation&apos;s status has been updated. </summary>
        public DateTimeOffset LastActionDateTimeUtc { get; }
        /// <summary> List of possible statuses for job or document. </summary>
        public Status? Status { get; }
        /// <summary> The Status Summary of the operation </summary>
        public StatusSummary Summary { get; }
    }
}
