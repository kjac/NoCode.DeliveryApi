using Kjac.NoCode.DeliveryApi.Repositories;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Kjac.NoCode.DeliveryApi.Models.Dtos;

[TableName(TableNames.FilterTable)]
[PrimaryKey("id")]
[ExplicitColumns]
internal class FilterDto : QueryDtoBase
{
    [Column(Name = "properties")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(512)]
    public string PropertyAliases { get; set; } = null!;

    [Column(Name = "match")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(20)]
    public string FilterMatchType { get; set; } = null!;
}
