import {UmbContextBase} from '@umbraco-cms/backoffice/class-api';
import {UmbContextToken} from '@umbraco-cms/backoffice/context-api';
import type {UmbControllerHost} from '@umbraco-cms/backoffice/controller-api';
import {PACKAGE_ALIAS} from "../constants.ts";
import {
  ClientModel, ClientsService, AddClientRequestModel, UpdateClientRequestModel,
  FilterListModel, FiltersService, AddFilterRequestModel, UpdateFilterRequestModel,
  SortersService, SortListModel, AddSortRequestModel, UpdateSortRequestModel
} from "../api";
import {tryExecuteAndNotify} from '@umbraco-cms/backoffice/resources';
import {LanguageService} from '@umbraco-cms/backoffice/external/backend-api';

export class WorkspaceContext extends UmbContextBase<WorkspaceContext> {

  private _filterListModel?: FilterListModel;
  private _sorterListModel?: SortListModel;
  private _clients?: Array<ClientModel>;

  constructor(host: UmbControllerHost) {
    super(host, NO_CODE_DELIVERY_API_CONTEXT_TOKEN);
  }

  async getFilters() {
    if (!this._filterListModel) {
      const {data} = await tryExecuteAndNotify(this, FiltersService.getNoCodeDeliveryApiFilter());
      this._filterListModel = data;
    }
    return this._filterListModel;
  }

  async addFilter(model: AddFilterRequestModel) {
    const {error} = await tryExecuteAndNotify(
      this,
      FiltersService.postNoCodeDeliveryApiFilter({
        requestBody: model
      })
    );

    return await this._reloadFiltersOnSuccess(!error);
  }

  async updateFilter(id: string, model: UpdateFilterRequestModel) {
    const {error} = await tryExecuteAndNotify(
      this,
      FiltersService.putNoCodeDeliveryApiFilterById({
        id: id,
        requestBody: model
      })
    );

    return await this._reloadFiltersOnSuccess(!error);
  }

  async deleteFilter(id: string) {
    const {error} = await tryExecuteAndNotify(
      this,
      FiltersService.deleteNoCodeDeliveryApiFilterById({id: id})
    );

    return await this._reloadFiltersOnSuccess(!error);
  }

  async getSorters() {
    if (!this._sorterListModel) {
      const {data} = await tryExecuteAndNotify(this, SortersService.getNoCodeDeliveryApiSort());
      this._sorterListModel = data;
    }
    return this._sorterListModel;
  }

  async addSorter(model: AddSortRequestModel) {
    const {error} = await tryExecuteAndNotify(
      this,
      SortersService.postNoCodeDeliveryApiSort({
        requestBody: model
      })
    );

    return await this._reloadSortersOnSuccess(!error);
  }

  async updateSorter(id: string, model: UpdateSortRequestModel) {
    const {error} = await tryExecuteAndNotify(
      this,
      SortersService.putNoCodeDeliveryApiSortById({
        id: id,
        requestBody: model
      })
    );

    return await this._reloadSortersOnSuccess(!error);
  }

  async deleteSorter(id: string) {
    const {error} = await tryExecuteAndNotify(
      this,
      SortersService.deleteNoCodeDeliveryApiSortById({id: id})
    );

    return await this._reloadSortersOnSuccess(!error);
  }

  async getClients() {
    if (!this._clients) {
      const {data} = await tryExecuteAndNotify(this, ClientsService.getNoCodeDeliveryApiClient());
      this._clients = data;
    }
    return this._clients;
  }

  async addClient(model: AddClientRequestModel) {
    const {error} = await tryExecuteAndNotify(
      this,
      ClientsService.postNoCodeDeliveryApiClient({
        requestBody: model
      })
    );

    return await this._reloadClientsOnSuccess(!error);
  }

  async updateClient(id: string, model: UpdateClientRequestModel) {
    const {error} = await tryExecuteAndNotify(
      this,
      ClientsService.putNoCodeDeliveryApiClientById({
        id: id,
        requestBody: model
      })
    );

    return await this._reloadClientsOnSuccess(!error);
  }

  async deleteClient(id: string) {
    const {error} = await tryExecuteAndNotify(
      this,
      ClientsService.deleteNoCodeDeliveryApiClientById({id: id})
    );

    return await this._reloadClientsOnSuccess(!error);
  }

  async getLanguages() {
    const {data} = await tryExecuteAndNotify(this, LanguageService.getLanguage({take: 100}));
    return data?.items;
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
