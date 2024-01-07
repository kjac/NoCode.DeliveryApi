using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Models.Dtos;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal sealed class SortRepository : QueryRepositoryBase<SortDto, SortModel>, ISortRepository
{
    public SortRepository(IScopeProvider scopeProvider, IRuntimeState runtimeState)
        : base(scopeProvider, runtimeState)
    {
    }

    protected override SortModel ParseModel(SortDto dto)
        => new SortModel
        {
            Key = dto.Key,
            Name = dto.Name,
            Alias = dto.Alias,
            PropertyAlias = dto.PropertyAlias,
            PrimitiveFieldType = ModelEnum<PrimitiveFieldType>(dto.PrimitiveFieldType),
            IndexFieldName = dto.IndexFieldName
        };

    protected override SortDto MapModelToDto(SortModel model, SortDto dto)
    {
        dto.Key = model.Key;
        dto.Name = model.Name;
        dto.Alias = model.Alias;
        dto.PropertyAlias = model.PropertyAlias;
        dto.PrimitiveFieldType = DtoEnum(model.PrimitiveFieldType);
        dto.IndexFieldName = model.IndexFieldName;

        return dto;
    }
}
