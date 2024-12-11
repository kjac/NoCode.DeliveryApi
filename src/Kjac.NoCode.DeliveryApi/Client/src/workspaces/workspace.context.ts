import {UmbContextBase} from '@umbraco-cms/backoffice/class-api';
import {UmbContextToken} from '@umbraco-cms/backoffice/context-api';
import type {UmbControllerHost} from '@umbraco-cms/backoffice/controller-api';
import {PACKAGE_ALIAS} from '../constants.ts';
import {ClientModel, ClientsService, FilterListModel, FiltersService, SortersService, SortListModel} from '../api';
import {tryExecuteAndNotify} from '@umbraco-cms/backoffice/resources';
import {LanguageService} from '@umbraco-cms/backoffice/external/backend-api';
import {FilterBase, FilterDetails} from './models/filter.ts';
import {SorterBase, SorterDetails} from './models/sorter.ts';
import {ClientBase, ClientDetails} from './models/client.ts';

export class WorkspaceContext extends UmbContextBase<WorkspaceContext> {

  private _filterListModel?: FilterListModel;
  private _sorterListModel?: SortListModel;
  private _clients?: Array<ClientModel>;

  constructor(host: UmbControllerHost) {
    super(host, NO_CODE_DELIVERY_API_CONTEXT_TOKEN);
  }

  async getFilters() {
    await this._ensureFilters();

    return this._filterListModel?.filters.map(filterModel=>
    {
      const filterDetails : FilterDetails = {
        id: filterModel.id,
        name: filterModel.name,
        alias: filterModel.alias,
        fieldName: filterModel.fieldName,
        propertyAliases: filterModel.propertyAliases,
        fieldType: filterModel.primitiveFieldType,
        matchType: filterModel.filterMatchType
      };
      return filterDetails;
    }) ?? [];
  }

  async canAddFilter() {
    await this._ensureFilters();

    return this._filterListModel?.canAddFilter ?? false;
  }

  async addFilter(filter: FilterBase) {
    const {error} = await tryExecuteAndNotify(
      this,
      FiltersService.postNoCodeDeliveryApiFilter({
        requestBody: {
          name: filter.name,
          propertyAliases: filter.propertyAliases,
          filterMatchType: filter.matchType,
          primitiveFieldType: filter.fieldType
        }
      })
    );

    return await this._reloadFiltersOnSuccess(!error);
  }

  async updateFilter(id: string, filter: FilterBase) {
    const {error} = await tryExecuteAndNotify(
      this,
      FiltersService.putNoCodeDeliveryApiFilterById({
        id: id,
        requestBody: {
          name: filter.name,
          propertyAliases: filter.propertyAliases
        }
      })
    );

    return await this._reloadFiltersOnSuccess(!error);
  }

  async deleteFilter(filter: FilterDetails) {
    if (!filter.id) {
      throw new Error("Filter must have an ID to be deleted");
    }

    const {error} = await tryExecuteAndNotify(
      this,
      FiltersService.deleteNoCodeDeliveryApiFilterById({id: filter.id})
    );

    return await this._reloadFiltersOnSuccess(!error);
  }

  async getSorters() {
    await this._ensureSorters();
    return this._sorterListModel?.sorts.map(sorterModel=>
    {
      const sorterDetails : SorterDetails = {
        id: sorterModel.id,
        name: sorterModel.name,
        alias: sorterModel.alias,
        fieldName: sorterModel.fieldName,
        propertyAlias: sorterModel.propertyAlias,
        fieldType: sorterModel.primitiveFieldType
      };
      return sorterDetails;
    }) ?? [];
  }

  async canAddSorter() {
    await this._ensureSorters();

    return this._sorterListModel?.canAddSort ?? false;
  }

  async addSorter(sorter: SorterBase) {
    const {error} = await tryExecuteAndNotify(
      this,
      SortersService.postNoCodeDeliveryApiSort({
        requestBody: {
          name: sorter.name,
          propertyAlias: sorter.propertyAlias,
          primitiveFieldType: sorter.fieldType
        }
      })
    );

    return await this._reloadSortersOnSuccess(!error);
  }

  async updateSorter(id: string, sorter: SorterBase) {
    const {error} = await tryExecuteAndNotify(
      this,
      SortersService.putNoCodeDeliveryApiSortById({
        id: id,
        requestBody: {
          name: sorter.name,
          propertyAlias: sorter.propertyAlias
        }
      })
    );

    return await this._reloadSortersOnSuccess(!error);
  }

  async deleteSorter(sorter: SorterDetails) {
    if (!sorter.id) {
      throw new Error("Sorter must have an ID to be deleted");
    }

    const {error} = await tryExecuteAndNotify(
      this,
      SortersService.deleteNoCodeDeliveryApiSortById({id: sorter.id})
    );

    return await this._reloadSortersOnSuccess(!error);
  }

  async getClients() {
    if (!this._clients) {
      const {data} = await tryExecuteAndNotify(this, ClientsService.getNoCodeDeliveryApiClient());
      this._clients = data;
    }
    return this._clients?.map(clientModel => {
      const clientDetails : ClientDetails = {
        id: clientModel.id,
        name: clientModel.name,
        culture: clientModel.culture,
        origin: clientModel.origin,
        previewUrlPath: clientModel.previewUrlPath,
        publishedUrlPath: clientModel.publishedUrlPath
      }
      return clientDetails
    }) ?? [];
  }

  async addClient(client: ClientBase) {
    const {error} = await tryExecuteAndNotify(
      this,
      ClientsService.postNoCodeDeliveryApiClient({
        requestBody: {
          name: client.name,
          culture: client.culture,
          origin: client.origin,
          previewUrlPath: client.previewUrlPath,
          publishedUrlPath: client.publishedUrlPath
        }
      })
    );

    return await this._reloadClientsOnSuccess(!error);
  }

  async updateClient(id: string, client: ClientBase) {
    const {error} = await tryExecuteAndNotify(
      this,
      ClientsService.putNoCodeDeliveryApiClientById({
        id: id,
        requestBody: {
          name: client.name,
          culture: client.culture,
          origin: client.origin,
          previewUrlPath: client.previewUrlPath,
          publishedUrlPath: client.publishedUrlPath
        }
      })
    );

    return await this._reloadClientsOnSuccess(!error);
  }

  async deleteClient(client: ClientDetails) {
    if (!client.id) {
      throw new Error("Client must have an ID to be deleted");
    }

    const {error} = await tryExecuteAndNotify(
      this,
      ClientsService.deleteNoCodeDeliveryApiClientById({id: client.id})
    );

    return await this._reloadClientsOnSuccess(!error);
  }

  async getLanguages() {
    const {data} = await tryExecuteAndNotify(this, LanguageService.getLanguage({take: 100}));
    return data?.items;
  }

  private async _ensureFilters() {
    if (this._filterListModel) {
      return;
    }

    const {data} = await tryExecuteAndNotify(this, FiltersService.getNoCodeDeliveryApiFilter());
    this._filterListModel = data;
  }

  private async _ensureSorters() {
    if (this._sorterListModel) {
      return;
    }

    const {data} = await tryExecuteAndNotify(this, SortersService.getNoCodeDeliveryApiSort());
    this._sorterListModel = data;
  }

  private async _reloadFiltersOnSuccess(success: boolean) {
    if (success) {
      this._filterListModel = undefined;
      await this.getFilters();
    }
    return success;
  }

  private async _reloadSortersOnSuccess(success: boolean) {
    if (success) {
      this._sorterListModel = undefined;
      await this.getSorters();
    }
    return success;
  }

  private async _reloadClientsOnSuccess(success: boolean) {
    if (success) {
      this._clients = undefined;
      await this.getClients();
    }
    return success;
  }
}

export const api = WorkspaceContext;

export const NO_CODE_DELIVERY_API_CONTEXT_TOKEN = new UmbContextToken<WorkspaceContext>(
  `${PACKAGE_ALIAS}.Workspace.Context`
);
