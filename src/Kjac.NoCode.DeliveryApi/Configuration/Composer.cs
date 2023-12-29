using Kjac.NoCode.DeliveryApi.Deployment;
using Kjac.NoCode.DeliveryApi.Handlers;
using Kjac.NoCode.DeliveryApi.OpenApi;
using Kjac.NoCode.DeliveryApi.Repositories;
using Kjac.NoCode.DeliveryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Kjac.NoCode.DeliveryApi.Configuration;

public sealed class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddSingleton<IFilterService, FilterService>();
        builder.Services.AddSingleton<ISortService, SortService>();
        builder.Services.AddSingleton<IFieldBufferService, FieldBufferService>();
        builder.Services.AddSingleton<IClientService, ClientService>();
        builder.Services.AddSingleton<ICorsPolicyService, CorsPolicyService>();
        builder.Services.AddSingleton<IDeployService, DeployService>();
        builder.Services.AddSingleton<IModelAliasGenerator, ModelAliasGenerator>();
        builder.Services.AddSingleton<IFilterRepository, FilterRepository>();
        builder.Services.AddSingleton<ISortRepository, SortRepository>();
        builder.Services.AddSingleton<IClientRepository, ClientRepository>();

        builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        builder.Services.AddCors(
            options => options.AddPolicy(
                Constants.CorsPolicyName,
                configure => configure
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST")
            )
        );
        builder.Services.Configure<UmbracoPipelineOptions>(options => options.AddFilter(new DeliveryApiCorsPipelineFilter()));

        builder.AddNotificationAsyncHandler<UmbracoApplicationStartingNotification, StartingNotificationHandler>();
        builder.AddNotificationAsyncHandler<UmbracoApplicationStartedNotification, StartedNotificationHandler>();
    }

    private class DeliveryApiCorsPipelineFilter : UmbracoPipelineFilter
    {
        public DeliveryApiCorsPipelineFilter()
            : base(nameof(DeliveryApiCorsPipelineFilter)) =>
            PostRouting = app => app.UseCors(Constants.CorsPolicyName);
    }
}
