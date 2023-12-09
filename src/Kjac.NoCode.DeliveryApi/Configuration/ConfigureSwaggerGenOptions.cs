using Kjac.NoCode.DeliveryApi.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kjac.NoCode.DeliveryApi.Configuration;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions swaggerGenOptions)
        => swaggerGenOptions.ParameterFilter<SwaggerDocumentationFilter>();
}