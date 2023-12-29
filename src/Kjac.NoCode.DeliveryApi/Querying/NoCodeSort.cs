﻿using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Querying;

public sealed class NoCodeSort : ISortHandler
{
    private static readonly char[] _sortOperatorChars = new[] { ':' };

    private readonly ISortService _sortService;

    public NoCodeSort(ISortService sortService)
        => _sortService = sortService;

    public bool CanHandle(string query)
    {
        var parts = Split(query);
        return parts.Length == 2 && _sortService.ExistsAsync(parts[0]).GetAwaiter().GetResult();
    }

    public SortOption BuildSortOption(string sort)
    {
        var parts = Split(sort);
        var name = parts.First();
        var direction = parts.Last();
        SortModel sortModel = _sortService.GetAsync(name).GetAwaiter().GetResult();

        return new SortOption
        {
            FieldName = sortModel.IndexFieldName,
            Direction = direction.StartsWith("asc") ? Direction.Ascending : Direction.Descending
        };
    }

    private static string[] Split(string query)
        => query.Split(_sortOperatorChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
