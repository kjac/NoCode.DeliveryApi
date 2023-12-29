using System.Text.RegularExpressions;
using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Querying;

public partial class NoCodeFilter : FilterBase, IFilterHandler
{
    private static readonly char[] _filterOperatorChars = new[] { ':', '>', '<' };

    private readonly IFilterService _filterService;

    public NoCodeFilter(IFilterService filterService)
        => _filterService = filterService;


    public bool CanHandle(string query)
    {
        if (query.IsNullOrWhiteSpace())
        {
            return false;
        }

        var parts = query.Split(_filterOperatorChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Length == 2 && _filterService.ExistsAsync(parts.First()).GetAwaiter().GetResult();
    }

    public FilterOption BuildFilterOption(string filter)
        => ParseFilterOption(
            filter,
            FilterParserRegex(),
            name => _filterService.GetAsync(name).GetAwaiter().GetResult().IndexFieldName);

    [GeneratedRegex("(?<name>[^><:]*)(?<operator>[><:!]*)(?<value>.*)", RegexOptions.IgnoreCase)]
    private static partial Regex FilterParserRegex();
}
