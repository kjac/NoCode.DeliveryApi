﻿using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Models;

internal sealed class BufferIndexFieldModel : IIndexFieldModel
{
    public required string IndexFieldName { get; init; }

    public required FieldType IndexFieldType { get; init; }
}
