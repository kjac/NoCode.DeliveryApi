using System.Text.RegularExpressions;
using Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing;
using Umbraco.Cms.Api.Delivery.Querying.Filters;

namespace NoCodeDeliveryApi.DeliveryApi.Querying;

public partial class LevelFilter : ContainsFilterBase
{
    protected override string FieldName => LevelIndexer.FieldName;

    protected override Regex QueryParserRegex => CreateLevelRegex();

    [GeneratedRegex("nocLevel(?<operator>[><:]{1,2})(?<value>.*)", RegexOptions.IgnoreCase)]
    public static partial Regex CreateLevelRegex();
}