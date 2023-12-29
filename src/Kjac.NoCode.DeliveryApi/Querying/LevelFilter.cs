using System.Text.RegularExpressions;
using Kjac.NoCode.DeliveryApi.Indexing;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Querying;

public partial class LevelFilter : FilterBase, IFilterHandler
{
    public bool CanHandle(string query)
        => LevelParserRegex().IsMatch(query);

    public FilterOption BuildFilterOption(string filter)
        => ParseFilterOption(filter, LevelParserRegex(), _ => LevelIndexer.FieldName);

    [GeneratedRegex("nocLevel(?<operator>[><:]{1,2})(?<value>.*)", RegexOptions.IgnoreCase)]
    private static partial Regex LevelParserRegex();
}
