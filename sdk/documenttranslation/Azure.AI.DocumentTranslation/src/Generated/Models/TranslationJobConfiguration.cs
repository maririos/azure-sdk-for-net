// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure.AI.DocumentTranslation.Models
{
    /// <summary> Definition for the input batch translation request. </summary>
    public partial class TranslationJobConfiguration
    {
        /// <summary> Initializes a new instance of TranslationJobConfiguration. </summary>
        /// <param name="source"> Source of the input documents. </param>
        /// <param name="targets"> Location of the destination for the output. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="source"/> or <paramref name="targets"/> is null. </exception>
        public TranslationJobConfiguration(SourceConfiguration source, IEnumerable<TargetConfiguration> targets)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (targets == null)
            {
                throw new ArgumentNullException(nameof(targets));
            }

            Source = source;
            Targets = targets.ToList();
        }

        /// <summary> Source of the input documents. </summary>
        public SourceConfiguration Source { get; }
        /// <summary> Location of the destination for the output. </summary>
        public IList<TargetConfiguration> Targets { get; }
        /// <summary> Storage type of the input documents source string. </summary>
        public StorageType? StorageType { get; set; }
    }
}
