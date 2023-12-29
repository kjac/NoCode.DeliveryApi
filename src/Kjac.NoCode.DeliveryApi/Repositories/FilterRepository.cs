using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Models.Dtos;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal class FilterRepository : QueryRepositoryBase<FilterDto, FilterModel>, IFilterRepository
{
    public FilterRepository(IScopeProvider scopeProvider, IRuntimeState runtimeState)
        : base(scopeProvider, runtimeState)
    {
    }

    protected override FilterModel ParseModel(FilterDto dto)
        => new FilterModel
        {
            Key = dto.Key,
            Name = dto.Name,
            Alias = dto.Alias,
            PropertyAliases = dto.PropertyAliases.Split(Umbraco.Cms.Core.Constants.CharArrays.Comma),
            FilterMatchType = ModelEnum<FilterMatchType>(dto.FilterMatchType),
            PrimitiveFieldType = ModelEnum<PrimitiveFieldType>(dto.PrimitiveFieldType),
            IndexFieldName = dto.IndexFieldName
        };

    protected override FilterDto MapModelToDto(FilterModel filterModel, FilterDto dto)
    {
        dto.Key = filterModel.Key;
        dto.Name = filterModel.Name;
        dto.Alias = filterModel.Alias;
        dto.PropertyAliases = string.Join(',', filterModel.PropertyAliases);
        dto.FilterMatchType = DtoEnum(filterModel.FilterMatchType);
        dto.PrimitiveFieldType = DtoEnum(filterModel.PrimitiveFieldType);
        dto.IndexFieldName = filterModel.IndexFieldName;

        return dto;
    }
}
