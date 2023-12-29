using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Kjac.NoCode.DeliveryApi.Models.Dtos;

internal abstract class QueryDtoBase : DtoBase
{
    [Column(Name = "alias")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(60)]
    public string Alias { get; set; } = null!;

    [Column(Name = "primitive")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(20)]
    public string PrimitiveFieldType { get; set; } = null!;

    [Column(Name = "fieldName")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(20)]
    public string IndexFieldName { get; set; } = null!;
}
