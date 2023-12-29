using System.Text.RegularExpressions;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Querying;

public abstract class FilterBase
{
    protected FilterOption ParseFilterOption(string filter, Regex regex, Func<string, string> indexFieldName)
    {
        Match match = regex.Match(filter);
        if (match.Success is false)
        {
            // ThisShouldNotHappen(tm) - return a bogus filter that will never match anything
            return BogusFilterOption();
        }

        FilterOperation? operation = ParseFilterOperation(match.Groups["operator"].Value);
        if (operation.HasValue is false)
        {
            // bad operator - return a bogus filter that will never match anything
            return BogusFilterOption();
        }

        var name = match.Groups["name"].Value;
        var values = match.Groups["value"].Value.Split(',');
        var fieldName = indexFieldName(name);

        return new FilterOption
        {
            FieldName = fieldName,
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
}
