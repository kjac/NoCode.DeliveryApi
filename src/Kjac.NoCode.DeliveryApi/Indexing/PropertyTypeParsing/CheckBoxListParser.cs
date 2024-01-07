using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class CheckBoxListParser : StringArrayParser
{
    public CheckBoxListParser(IJsonSerializer jsonSerializer)
        : base(jsonSerializer)
    {
    }
}
