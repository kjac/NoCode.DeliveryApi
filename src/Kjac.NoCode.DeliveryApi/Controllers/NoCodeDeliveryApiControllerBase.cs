using Kjac.NoCode.DeliveryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Web.Common.Authorization;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[VersionedApiBackOfficeRoute("no-code-delivery-api")]
[MapToApi("no-code-delivery-api")]
[Authorize(Policy = AuthorizationPolicies.SectionAccessSettings)]
public abstract class NoCodeDeliveryApiControllerBase : ManagementApiControllerBase
{
    protected IActionResult OperationStatusResult(OperationStatus operationStatus)
        => operationStatus == OperationStatus.Success
            ? Ok()
            : OperationStatusResult(operationStatus, builder => operationStatus switch
                {
                    OperationStatus.DuplicateAlias => BadRequest(builder
                        .WithTitle("Duplicate alias")
                        .WithDetail("Please ensure unique aliases for all items.")
                        .Build()
                    ),
                    OperationStatus.NotFound => NotFound(builder
                        .WithTitle("The item was not found")
                        .WithDetail("Could not find the target item for the operation.")
                        .Build()
                    ),
                    OperationStatus.UnknownIndexFieldName => BadRequest(builder
                        .WithTitle("Unknown index field name")
                        .WithDetail("The supplied index field was not recognized.")
                        .Build()
                    ),
                    OperationStatus.FailedCreate => BadRequest(builder
                        .WithTitle("Creation failed")
                        .WithDetail("Could not perform the create action. Check the logs for details.")
                        .Build()
                    ),
                    OperationStatus.FailedUpdate => BadRequest(builder
                        .WithTitle("Update failed")
                        .WithDetail("Could not perform the update action. Check the logs for details.")
                        .Build()
                    ),
                    OperationStatus.FailedDelete => BadRequest(builder
                        .WithTitle("Delete failed")
                        .WithDetail("Could not perform the delete action. Check the logs for details.")
                        .Build()
                    ),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, "Unknown operation status.")
                }
            );
}
