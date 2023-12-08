namespace Kjac.NoCode.DeliveryApi.Services;

internal static class IndexFieldName
{
    private static string RandomIndexFieldName() => Guid.NewGuid().ToString("D").Split('-').First();

    public static string GetRandom()
        => Get(RandomIndexFieldName());

    public static string Get(string fieldName)
        => $"noc{fieldName}";
}