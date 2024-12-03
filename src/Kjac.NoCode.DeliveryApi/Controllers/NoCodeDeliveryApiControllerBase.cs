using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[VersionedApiBackOfficeRoute("no-code-delivery-api")]
[MapToApi("no-code-delivery-api")]
public abstract class NoCodeDeliveryApiControllerBase : ManagementApiControllerBase
{
}
