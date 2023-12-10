using Kjac.NoCode.DeliveryApi.OpenApi;
using Kjac.NoCode.DeliveryApi.Repositories;
using Kjac.NoCode.DeliveryApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Kjac.NoCode.DeliveryApi.Composing;

public sealed class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddSingleton<IFilterService, FilterService>();
        builder.Services.AddSingleton<ISortService, SortService>();
        builder.Services.AddSingleton<IFieldBufferService, FieldBufferService>();
        builder.Services.AddSingleton<IModelAliasGenerator, ModelAliasGenerator>();
        builder.Services.AddSingleton<IFilterRepository, FilterRepository>();
        builder.Services.AddSingleton<ISortRepository, SortRepository>();
        builder.Services.AddSingleton<IModelAliasGenerator, ModelAliasGenerator>();

        builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
    }
}
