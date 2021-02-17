// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;

namespace Azure.AI.DocumentTranslation
{
    internal class DocumentTranslationHelpers
    {
        internal static void ExtractTopAndSkip(string nextLink, out int top, out int skip)
        {
            top = default;
            skip = default;

            // Extracting parameters from the URL.
            // nextLink - https://westus.api.cognitive.microsoft.com/translator/text/batch/v1.0-preview.1/batches/8002878d-2e43-4675-ad20-455fe004641b/documents?$skip=20&$top=0

            string @params = nextLink.Split('?').Last();
            // params = '$skip=20&$top=0'

            // Extracting Top and Skip parameter values
            string[] parameters = @params.Split('&');
            // '$skip=20', '$top=0'

            foreach (string paramater in parameters)
            {
                if (paramater.Contains("top"))
                {
                    _ = int.TryParse(paramater.Split('=')[1], out top);
                    // 0
                }
                if (paramater.Contains("skip"))
                {
                    _ = int.TryParse(paramater.Split('=')[1], out skip);
                    // 20
                }
            }
        }
    }
}
