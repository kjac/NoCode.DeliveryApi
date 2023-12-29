using Kjac.NoCode.DeliveryApi.Repositories;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Kjac.NoCode.DeliveryApi.Models.Dtos;

[TableName(TableNames.ClientTable)]
[PrimaryKey("id")]
[ExplicitColumns]
internal class ClientDto : DtoBase
{
    [Column(Name = "origin")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(256)]
    public string Origin { get; set; } = null!;
}
