using System.Text.RegularExpressions;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal partial class MarkdownParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string stringValue)
        {
            return null;
        }

        var valueWithoutMarkdownChars = MarkdownCleanupRegex().Replace(stringValue, " ");
        return new object[] { WhitespaceCleanupRegex().Replace(valueWithoutMarkdownChars, " ") };
    }

    [GeneratedRegex(@"[#=*_>.,0-9\-!\[\]\(\)`@\/:""]")]
    private static partial Regex MarkdownCleanupRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceCleanupRegex();
}
