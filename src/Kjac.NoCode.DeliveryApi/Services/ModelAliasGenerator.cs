using Umbraco.Cms.Core.Strings;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class ModelAliasGenerator : IModelAliasGenerator
{
    private readonly IShortStringHelper _shortStringHelper;

    public ModelAliasGenerator(IShortStringHelper shortStringHelper)
        => _shortStringHelper = shortStringHelper;

    public string CreateAlias(string name)
        => $"noc{_shortStringHelper.CleanString(name, CleanStringType.Alias | CleanStringType.PascalCase, "en-US")}";
}
