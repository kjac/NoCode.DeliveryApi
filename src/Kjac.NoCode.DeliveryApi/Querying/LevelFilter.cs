using System.Text.RegularExpressions;
using Kjac.NoCode.DeliveryApi.Indexing;
using Umbraco.Cms.Api.Delivery.Querying.Filters;

namespace Kjac.NoCode.DeliveryApi.Querying;

public partial class LevelFilter : ContainsFilterBase
{
    protected override string FieldName => LevelIndexer.FieldName;

    protected override Regex QueryParserRegex => CreateLevelRegex();

    [GeneratedRegex("nocLevel(?<operator>[><:]{1,2})(?<value>.*)", RegexOptions.IgnoreCase)]
    private static partial Regex CreateLevelRegex();
}
