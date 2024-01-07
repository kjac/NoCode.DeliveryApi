using Kjac.NoCode.DeliveryApi.Repositories;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Kjac.NoCode.DeliveryApi.Models.Dtos;

[TableName(TableNames.SortTable)]
[PrimaryKey("id")]
[ExplicitColumns]
internal sealed class SortDto : QueryDtoBase
{
    [Column(Name = "property")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(512)]
    public string PropertyAlias { get; set; } = null!;
}
