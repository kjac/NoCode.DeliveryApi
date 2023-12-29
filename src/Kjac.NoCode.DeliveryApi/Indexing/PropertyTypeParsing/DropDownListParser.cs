using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal class DropDownListParser : StringArrayParser
{
    public DropDownListParser(IJsonSerializer jsonSerializer)
        : base(jsonSerializer)
    {
    }
}
