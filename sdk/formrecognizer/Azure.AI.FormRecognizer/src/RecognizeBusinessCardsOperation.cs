// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.AI.FormRecognizer.Models
{
    /// <summary>
    /// Tracks the status of a long-running operation for recognizing values from business cards.
    /// </summary>
    public class RecognizeBusinessCardsOperation : RecognizePrebuiltModelOperation
    {
        internal RecognizeBusinessCardsOperation(FormRecognizerRestClient allOperations, ClientDiagnostics diagnostics, string location) : base(allOperations, diagnostics, location, FormRecognizerPrebuiltModel.BusinessCard) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecognizeBusinessCardsOperation"/> class which
        /// tracks the status of a long-running operation for training a model from a collection of custom forms.
        /// </summary>
        /// <param name="operationId">The ID of this operation.</param>
        /// <param name="client">The client used to check for completion.</param>
        public RecognizeBusinessCardsOperation(string operationId, FormRecognizerClient client) : base(operationId, client, FormRecognizerPrebuiltModel.BusinessCard) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecognizeBusinessCardsOperation"/> class. This constructor
        /// is intended to be used for mocking only.
        /// </summary>
        protected RecognizeBusinessCardsOperation() : base() { }
    }
}
