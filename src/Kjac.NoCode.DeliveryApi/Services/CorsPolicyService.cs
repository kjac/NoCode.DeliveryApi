using Kjac.NoCode.DeliveryApi.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class CorsPolicyService : ICorsPolicyService
{
    private readonly CorsOptions _options;
    private readonly IClientService _clientService;
    private readonly ILogger<CorsPolicyService> _logger;

    public CorsPolicyService(IOptions<CorsOptions> options, IClientService clientService, ILogger<CorsPolicyService> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        _clientService = clientService;
        _logger = logger;
        _options = options.Value;
    }

    public async Task ApplyClientOriginsAsync()
    {
        CorsPolicy? policy = _options.GetPolicy(Constants.CorsPolicyName);
        if (policy is null)
        {
            _logger.LogWarning(
                "Could not find CORS policy: {CorsPolicyName} - automatic CORS handling is disabled for the Delivery API.",
                Constants.CorsPolicyName);
            return;
        }

        IEnumerable<ClientModel> clients = await _clientService.GetAllAsync();
        var origins = clients.Select(client => client.Origin).ToArray();

        policy.Origins.Clear();
        foreach (var origin in origins)
        {
            policy.Origins.Add(origin);
        }
    }
}
