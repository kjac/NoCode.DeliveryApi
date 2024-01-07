using Kjac.NoCode.DeliveryApi.Repositories;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Kjac.NoCode.DeliveryApi.Models.Dtos;

[TableName(TableNames.ClientTable)]
[PrimaryKey("id")]
[ExplicitColumns]
internal sealed class ClientDto : DtoBase
{
    [Column(Name = "origin")]
    [NullSetting(NullSetting = NullSettings.NotNull)]
    [Length(256)]
    public string Origin { get; set; } = null!;

    [Column(Name = "previewUrlPath")]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(256)]
    public string? PreviewUrlPath { get; set; }

    [Column(Name = "publishedUrlPath")]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(256)]
    public string? PublishedUrlPath { get; set; }

    [Column(Name = "culture")]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(10)]
    public string? Culture { get; set; }
}
