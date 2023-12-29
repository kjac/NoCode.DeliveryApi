using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Querying;

public sealed class IsNotFilter : IFilterHandler
{
    private const string FilterSpecifier = "nocIsNot:";

    public bool CanHandle(string query)
        => query.StartsWith(FilterSpecifier, StringComparison.OrdinalIgnoreCase);

    public FilterOption BuildFilterOption(string filter)
    {
        var fieldValue = filter[FilterSpecifier.Length..];

        return new FilterOption
        {
            // NOTE: this field is part of the core indexing, so we'll reuse it here
            FieldName = "itemId",
            // NOTE: it does not make any sense to have multiple values here, as it would end up returning everything
            Values = new [] { fieldValue },
            Operator = FilterOperation.IsNot
        };
    }
}
