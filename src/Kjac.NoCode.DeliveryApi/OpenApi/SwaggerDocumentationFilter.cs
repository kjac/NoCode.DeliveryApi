using System.Text;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Api.Delivery.Controllers.Content;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.OpenApi;

public class SwaggerDocumentationFilter : IParameterFilter
{
    private const string QueryParamsDocsUrl = "https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters";

    private readonly IFilterService _filterService;
    private readonly ISortService _sortService;

    public SwaggerDocumentationFilter(IFilterService filterService, ISortService sortService)
    {
        _filterService = filterService;
        _sortService = sortService;
    }

    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (context.ParameterInfo.Member.DeclaringType?.Implements<ContentApiControllerBase>() is not true)
        {
            return;
        }

        switch (parameter.Name)
        {
            case "filter":
                AddFilterDocumentation(parameter);
                break;
            case "sort":
                AddSortDocumentation(parameter);
                break;
        }
    }

    private void AddFilterDocumentation(OpenApiParameter parameter)
    {
        IEnumerable<FilterModel> filters = _filterService.GetAllAsync().GetAwaiter().GetResult();
        foreach (FilterModel filter in filters)
        {
            parameter.Examples[$"No-Code: {filter.Name}"] = new OpenApiExample
            {
                Description = FilterDescription(filter),
                Value = new OpenApiArray
                {
                    new OpenApiString($"{filter.Alias}:{FilterValue(filter)}")
                }
            };
        }

        parameter.Examples["No-Code: Is Not"] = new OpenApiExample
        {
            Description = "This filter ensures that a specific content item (identified by its key) is excluded from the query result.",
            Value = new OpenApiArray
            {
                new OpenApiString($"nocIsNot:content-key")
            }
        };

        parameter.Examples["No-Code: Level"] = new OpenApiExample
        {
            Description = $"This filter allows for querying the content level in the content tree.\n\nUse whole numbers (integers) only.\n\nMultiple values can be supplied, causing the filter to perform a logical OR between these values. Use a comma to separate values, e.g. `nocLevel:2,3,4`.{RangeSyntaxDescription()}{ReadMoreDescription()}",
            Value = new OpenApiArray
            {
                new OpenApiString($"nocLevel:2")
            }
        };
    }

    private string FilterDescription(FilterModel filterModel)
    {
        var description = new StringBuilder();
        description.Append(
            filterModel.IndexFieldType switch
            {
                FieldType.StringRaw => "This filter performs exact matching against string values. Use it to filter against known identifiers (e.g. content keys or product SKUs).",
                FieldType.StringAnalyzed => "This filter performs partial matching against string values. Use it for textual (wildcard) searches.",
                FieldType.Number => "This filter performs matching against numeric values. It can be used for both exact and range filtering.\n\nUse whole numbers (integers) only.",
                FieldType.Date => "This filter performs matching against date/time values. It can be used for both exact and range filtering.\n\nUse the ISO 8601 format (yyyy-MM-ddTHH:mm:ss). The time component can be omitted to perform date-only filtering.",
                _ => throw new ArgumentOutOfRangeException(nameof(FilterModel.IndexFieldType))
            }
        );

        description.Append(
            filterModel.PropertyAliases.Count() == 1
                ? $"\n\nThe filter is applied to the content property `{filterModel.PropertyAliases.First()}`"
                : $"\n\nThe filter is applied to these content properties:\n{string.Join("\n", filterModel.PropertyAliases.Select(alias => $"* `{alias}`"))}"
        );

        if (filterModel.IndexFieldType is not FieldType.StringAnalyzed)
        {
            description.Append($"\n\nMultiple values can be supplied, causing the filter to perform a logical OR between these values. Use a comma to separate values, e.g. `{filterModel.Alias}:value1,value2`.");
        }

        description.Append(filterModel.IndexFieldType is FieldType.Date or FieldType.Number
            ? RangeSyntaxDescription()
            : BasicSyntaxDescription());

        description.Append(ReadMoreDescription());

        return description.ToString();
    }

    private static string BasicSyntaxDescription() => "\n\nThe filter supports the following operators:\n* Use `:` for \"equal\".\n* Use `:!` for \"not equal\".";

    private static string RangeSyntaxDescription() => $"{BasicSyntaxDescription()}\n* Use `>` for \"greater than\".\n* Use `>:` for \"greater than or equal\".\n* Use `<` for \"less than\".\n* Use `<:` for \"less than or equal\".";

    private static string ReadMoreDescription() => $"\n\nRead more about filtering and the filter syntax in [the Umbraco documentation]({QueryParamsDocsUrl})";

    private string FilterValue(FilterModel filterModel)
        => filterModel.IndexFieldType switch
        {
            FieldType.StringRaw => "some-identifier",
            FieldType.StringAnalyzed => "text",
            FieldType.Number => "1234",
            FieldType.Date => "2023-05-20T12:30:00",
            _ => throw new ArgumentOutOfRangeException(nameof(FilterModel.IndexFieldType))
        };

    private void AddSortDocumentation(OpenApiParameter parameter)
    {
        IEnumerable<SortModel> sorts = _sortService.GetAllAsync().GetAwaiter().GetResult();
        foreach (SortModel sort in sorts)
        {
            parameter.Examples[$"No-Code: {sort.Name} (ascending)"] = new OpenApiExample
            {
                Description = SortDescription(sort),
                Value = new OpenApiArray
                {
                    new OpenApiString($"{sort.Alias}:asc")
                }
            };

            parameter.Examples[$"No-Code: {sort.Name} (descending)"] = new OpenApiExample
            {
                Description = SortDescription(sort),
                Value = new OpenApiArray
                {
                    new OpenApiString($"{sort.Alias}:desc")
                }
            };
        }
    }

    private string SortDescription(SortModel sortModel)
    {
        var description = new StringBuilder();
        description.Append(
            sortModel.IndexFieldType switch
            {
                FieldType.StringSortable => "This sorter performs alpha-numeric sorting.",
                FieldType.Number => "This sorter performs numeric sorting.",
                FieldType.Date => "This sorter performs sorting by date.",
                _ => throw new ArgumentOutOfRangeException(nameof(FilterModel.IndexFieldType))
            }
        );

        description.Append(" Use `asc` or `desc` to sort ascending or descending, respectively.");

        description.Append($"\n\nThe sorting is performed on the content property `{sortModel.PropertyAlias}`");

        description.Append($"\n\nRead more about sorting in [the Umbraco documentation]({QueryParamsDocsUrl})");

        return description.ToString();
    }
}
