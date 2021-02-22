// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Core.TestFramework
{
    public class DocumentTranslationRecordedTestSanitizer : RecordedTestSanitizer
    {
        public DocumentTranslationRecordedTestSanitizer()
        {
            SanitizedHeaders.Add("Ocp-Apim-Subscription-Key");
        }
    }
}
