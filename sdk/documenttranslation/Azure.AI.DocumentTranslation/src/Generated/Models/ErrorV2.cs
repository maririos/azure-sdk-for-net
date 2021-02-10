// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.AI.DocumentTranslation.Models
{
    /// <summary> This contains an outer error with error code, message, details, target and an inner error with more descriptive details. </summary>
    public partial class ErrorV2
    {
        /// <summary> Initializes a new instance of ErrorV2. </summary>
        internal ErrorV2()
        {
        }

        /// <summary> Initializes a new instance of ErrorV2. </summary>
        /// <param name="code"> Enums containing high level error codes. </param>
        /// <param name="message"> Gets high level error message. </param>
        /// <param name="target">
        /// Gets the source of the error.
        /// 
        /// For example it would be &quot;documents&quot; or &quot;document id&quot; in case of invalid document.
        /// </param>
        /// <param name="innerError">
        /// New Inner Error format which conforms to Cognitive Services API Guidelines which is available at https://microsoft.sharepoint.com/%3Aw%3A/t/CognitiveServicesPMO/EUoytcrjuJdKpeOKIK_QRC8BPtUYQpKBi8JsWyeDMRsWlQ?e=CPq8ow.
        /// 
        /// This contains required properties ErrorCode, message and optional properties target, details(key value pair), inner error(this can be nested).
        /// </param>
        internal ErrorV2(ErrorCodeV2? code, string message, string target, InnerErrorV2 innerError)
        {
            Code = code;
            Message = message;
            Target = target;
            InnerError = innerError;
        }

        /// <summary> Enums containing high level error codes. </summary>
        public ErrorCodeV2? Code { get; }
        /// <summary> Gets high level error message. </summary>
        public string Message { get; }
        /// <summary>
        /// Gets the source of the error.
        /// 
        /// For example it would be &quot;documents&quot; or &quot;document id&quot; in case of invalid document.
        /// </summary>
        public string Target { get; }
        /// <summary>
        /// New Inner Error format which conforms to Cognitive Services API Guidelines which is available at https://microsoft.sharepoint.com/%3Aw%3A/t/CognitiveServicesPMO/EUoytcrjuJdKpeOKIK_QRC8BPtUYQpKBi8JsWyeDMRsWlQ?e=CPq8ow.
        /// 
        /// This contains required properties ErrorCode, message and optional properties target, details(key value pair), inner error(this can be nested).
        /// </summary>
        public InnerErrorV2 InnerError { get; }
    }
}
