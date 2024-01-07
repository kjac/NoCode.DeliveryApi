using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class DropDownListParser : StringArrayParser
{
    public DropDownListParser(IJsonSerializer jsonSerializer)
        : base(jsonSerializer)
    {
    }
}
