// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.AI.DocumentTranslation.Models
{
    public partial class StatusSummary
    {
        internal static StatusSummary DeserializeStatusSummary(JsonElement element)
        {
            Optional<int> total = default;
            Optional<int> failed = default;
            Optional<int> success = default;
            Optional<int> inProgress = default;
            Optional<int> notYetStarted = default;
            Optional<int> cancelled = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("total"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    total = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("failed"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    failed = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("success"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    success = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("inProgress"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    inProgress = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("notYetStarted"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    notYetStarted = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("cancelled"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    cancelled = property.Value.GetInt32();
                    continue;
                }
            }
            return new StatusSummary(Optional.ToNullable(total), Optional.ToNullable(failed), Optional.ToNullable(success), Optional.ToNullable(inProgress), Optional.ToNullable(notYetStarted), Optional.ToNullable(cancelled));
        }
    }
}
