// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace Azure.Messaging.EventGrid.SystemEvents
{
    /// <summary> Schema of the Data property of an EventGridEvent for an Microsoft.Communication.SMSDeliveryReportReceived event. </summary>
    public partial class AcssmsDeliveryReportReceivedEventData : AcssmsEventBaseProperties
    {
        /// <summary> Initializes a new instance of AcssmsDeliveryReportReceivedEventData. </summary>
        internal AcssmsDeliveryReportReceivedEventData()
        {
            DeliveryAttempts = new ChangeTrackingList<AcssmsDeliveryAttemptProperties>();
        }

        /// <summary> Initializes a new instance of AcssmsDeliveryReportReceivedEventData. </summary>
        /// <param name="messageId"> The identity of the SMS message. </param>
        /// <param name="from"> The identity of SMS message sender. </param>
        /// <param name="to"> The identity of SMS message receiver. </param>
        /// <param name="deliveryStatus"> Status of Delivery. </param>
        /// <param name="deliveryStatusDetails"> Details about Delivery Status. </param>
        /// <param name="deliveryAttempts"> List of details of delivery attempts made. </param>
        /// <param name="receivedTimestamp"> The time at which the SMS delivery report was received. </param>
        internal AcssmsDeliveryReportReceivedEventData(string messageId, string @from, string to, string deliveryStatus, string deliveryStatusDetails, IReadOnlyList<AcssmsDeliveryAttemptProperties> deliveryAttempts, DateTimeOffset? receivedTimestamp) : base(messageId, @from, to)
        {
            DeliveryStatus = deliveryStatus;
            DeliveryStatusDetails = deliveryStatusDetails;
            DeliveryAttempts = deliveryAttempts;
            ReceivedTimestamp = receivedTimestamp;
        }

        /// <summary> Status of Delivery. </summary>
        public string DeliveryStatus { get; }
        /// <summary> Details about Delivery Status. </summary>
        public string DeliveryStatusDetails { get; }
        /// <summary> List of details of delivery attempts made. </summary>
        public IReadOnlyList<AcssmsDeliveryAttemptProperties> DeliveryAttempts { get; }
        /// <summary> The time at which the SMS delivery report was received. </summary>
        public DateTimeOffset? ReceivedTimestamp { get; }
    }
}