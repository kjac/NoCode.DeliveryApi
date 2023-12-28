using Kjac.NoCode.DeliveryApi.Models.Dtos;
using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Infrastructure.Migrations;

namespace Kjac.NoCode.DeliveryApi.Migrations.V1_0;

internal class AddInitialTablesMigration : MigrationBase
{
    public AddInitialTablesMigration(IMigrationContext context)
        : base(context)
    {
    }

    protected override void Migrate()
    {
        if (TableExists(TableNames.FilterTable) is false)
        {
            Create.Table<FilterDto>().Do();
        }
        if (TableExists(TableNames.SortTable) is false)
        {
            Create.Table<SortDto>().Do();
        }
        if (TableExists(TableNames.ClientTable) is false)
        {
            Create.Table<ClientDto>().Do();
        }
    }
}