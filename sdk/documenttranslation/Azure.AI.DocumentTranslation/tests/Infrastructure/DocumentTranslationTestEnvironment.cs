// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.TestFramework;

namespace Azure.AI.DocumentTranslation.Tests
{
    public class DocumentTranslationTestEnvironment : TestEnvironment
    {
        public string Endpoint => GetRecordedVariable("DOCUMENT_TRANSLATION_ENDPOINT");
        public string ApiKey => GetRecordedVariable("DOCUMENT_TRANSLATION_API_KEY", options => options.IsSecret());
        public string SourceUrl => GetRecordedVariable("DOCUMENT_TRANSLATION_SOURCE_URL", options => options.IsSecret());
        public string TargetUrl => GetRecordedVariable("DOCUMENT_TRANSLATION_TARGET_URL", options => options.IsSecret());
    }
}
