using System.Text.RegularExpressions;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class MarkdownParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string stringValue)
        {
            return null;
        }
        var valueWithoutMarkdownChars = Regex.Replace(stringValue, @"[#=*_>.,0-9\-!\[\]\(\)`@\/:""]", " ");
        return new object[] { Regex.Replace(valueWithoutMarkdownChars, @"\s+", " ") };
    }
}