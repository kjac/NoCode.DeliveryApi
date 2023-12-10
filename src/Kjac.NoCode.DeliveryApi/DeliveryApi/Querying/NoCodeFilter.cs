using System.Text.RegularExpressions;
using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Querying;

public partial class NoCodeFilter : IFilterHandler
{
    private static readonly char[] FilterOperatorChars = new[] { ':', '>', '<' };
    
    private readonly IFilterService _filterService;

    public NoCodeFilter(IFilterService filterService)
        => _filterService = filterService;


    public bool CanHandle(string query)
    {
        if (query.IsNullOrWhiteSpace())
        {
            return false;
        }

        var parts = query.Split(FilterOperatorChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Length == 2 && _filterService.ExistsAsync(parts.First()).GetAwaiter().GetResult();
    }

    public FilterOption BuildFilterOption(string filter)
    {
        var match = FilterParserRegex().Match(filter);
        if (match.Success is false)
        {
            // ThisShouldNotHappen(tm) - return a bogus filter that will never match anything
            return BogusFilterOption();
        }

        var operation = ParseFilterOperation(match.Groups["operator"].Value);
        if (operation.HasValue is false)
        {
            // bad operator - return a bogus filter that will never match anything
            return BogusFilterOption();
        }
        
        var name = match.Groups["name"].Value;
        var values = match.Groups["value"].Value.Split(',');
        var filterModel = _filterService.GetAsync(name).GetAwaiter().GetResult();

        return new FilterOption
        {
            FieldName = filterModel.IndexFieldName,
            Operator = operation.Value,
            Values = values
        };

        FilterOption BogusFilterOption()
        {
            return new FilterOption
            {
                FieldName = "EE7EC8F1E1D643D4BCAD324E2725519E",
                Operator = FilterOperation.Is,
                Values = new[] { "CF3EC2C75F234379B2E262B4E384E494" }
            };
        }
    }
    
    private FilterOperation? ParseFilterOperation(string filterOperation)
        => filterOperation switch
        {
            ":" => FilterOperation.Is,
            ":!" => FilterOperation.IsNot,
            ">" => FilterOperation.GreaterThan,
            ">:" => FilterOperation.GreaterThanOrEqual,
            "<" => FilterOperation.LessThan,
            "<:" => FilterOperation.LessThanOrEqual,
            _ => null
        };

    [GeneratedRegex("(?<name>[^><:]*)(?<operator>[><:!]*)(?<value>.*)", RegexOptions.IgnoreCase)]
    public static partial Regex FilterParserRegex();
}