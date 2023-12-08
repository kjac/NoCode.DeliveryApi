using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Querying;

public sealed class IsNotFilter : IFilterHandler
{
    private const string FilterSpecifier = "nocIsNot:";

    public bool CanHandle(string query)
        => query.StartsWith(FilterSpecifier, StringComparison.OrdinalIgnoreCase);

    public FilterOption BuildFilterOption(string filter)
    {
        var fieldValue = filter[FilterSpecifier.Length..];

        // there might be several values for the filter
        var values = fieldValue.Split(',');

        return new FilterOption
        {
            // NOTE: this field is part of the core indexing, so we'll reuse it here. we really should add our own
            //       field to the index so we're not dependent on the core indexing, but this works for now. 
            FieldName = "itemId",
            Values = values,
            Operator = FilterOperation.IsNot
        };
    }
}