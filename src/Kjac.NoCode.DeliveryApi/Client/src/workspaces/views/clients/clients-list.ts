import { LitElement, html, customElement, css, state, when, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { ClientsService, ClientViewModel } from '../../../api';
import { UMB_CONFIRM_MODAL, UMB_MODAL_MANAGER_CONTEXT } from '@umbraco-cms/backoffice/modal';
import { CLIENT_MODAL_TOKEN } from './edit-client.ts';
import { LanguageResponseModel, LanguageService } from '@umbraco-cms/backoffice/external/backend-api';

@customElement('no-code-delivery-api-clients-workspace-view')
export default class ClientsWorkspaceViewElement extends UmbElementMixin(LitElement) {
  #modalManagerContext?: typeof UMB_MODAL_MANAGER_CONTEXT.TYPE;

  @state()
  private _clients?: Array<ClientViewModel>;

  private _languages?: Array<LanguageResponseModel>

  constructor() {
    super();
    this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (instance) => {
      this.#modalManagerContext = instance;
    });
  }

  async connectedCallback() {
    super.connectedCallback();
    await this._loadData();
  }

  render() {
    if (!this._clients) {
      return html`
        <umb-body-layout>
          <uui-loader></uui-loader>
        </umb-body-layout>
      `;
    }
    return html`
      <umb-body-layout header-transparent>
        <uui-button look="outline" label="Add a new client" @click=${this._addClient} slot="header"></uui-button>
        <uui-box>
          ${when(
            this._clients.length > 0,
            () => html`
              <uui-table>
                <uui-table-head>
                  <uui-table-head-cell></uui-table-head-cell>
                  <uui-table-head-cell>Name</uui-table-head-cell>
                  <uui-table-head-cell>Origin</uui-table-head-cell>
                  <uui-table-head-cell></uui-table-head-cell>
                </uui-table-head>
                ${repeat(
              this._clients!,
              (client) => client.id,
              (client) => html`
               <uui-table-row>
                 <uui-table-cell><uui-icon name="icon-unplug" aria-hidden="true"></uui-icon></uui-table-cell>
                 <uui-table-cell>${client.name}</uui-table-cell>
                 <uui-table-cell>${client.origin}</uui-table-cell>
                 <uui-table-cell style="text-align: right;">
                   <uui-action-bar>
                     <uui-button label="Edit client" look="secondary" @click=${() => this._editClient(client)}>
                       <uui-icon name="edit"></uui-icon>
                     </uui-button>
                     <uui-button label="Delete client" look="secondary" @click=${() => this._deleteClient(client)}>
                       <uui-icon name="delete"></uui-icon>
                     </uui-button>
                   </uui-action-bar>
                 </uui-table-cell>
               </uui-table-row>
             `)}
              </uui-table>
            `,
            () => html`<p>No clients have been added yet.</p>`
          )}
        </uui-box>
      </umb-body-layout>
    `;
  }

  private async _loadData() {
    const { data, error } = await ClientsService.getUmbracoManagementApiV1NoCodeDeliveryApiClient();
    if (error) {
      console.error(error);
      return;
    }

    this._clients = data;
  }

  private _addClient = () => this._editClient();

  private async _editClient(client?: ClientViewModel) {
    if (!this._languages) {
      this._languages = (await LanguageService.getLanguage({take: 100})).items;
    }

    const headline = client ? 'Edit client': 'Add client';
    const modalContext = this.#modalManagerContext?.open(
      this,
      CLIENT_MODAL_TOKEN,
      {
        data: {
          headline: headline,
          client: client,
          languages: this._languages
        }
      }
    );
    modalContext
      ?.onSubmit()
      .then(async value => {
        const result = client
          ? await ClientsService.putUmbracoManagementApiV1NoCodeDeliveryApiClientById({
            path: {
              id: client.id
            },
            body: {
              name: value.client.name,
              origin: value.client.origin,
              culture: value.client.culture,
              previewUrlPath: value.client.previewUrlPath,
              publishedUrlPath: value.client.publishedUrlPath
            }
          })
          : await ClientsService.postUmbracoManagementApiV1NoCodeDeliveryApiClient( { body: value.client });

        if (!result.response.ok) {
          console.error('Unable to edit client - response code was: ', result.response.status);
          return;
        }

        await this._loadData();
      })
      .catch(() => {
        // the modal was cancelled, do nothing
      });
  }

  private _deleteClient(client: ClientViewModel) {
    const modalContext = this.#modalManagerContext?.open(
      this,
      UMB_CONFIRM_MODAL,
      {
        data: {
          headline: 'Delete client',
          content: `Are you sure you want to delete the client "${client.name}"?`,
          color: 'danger',
          confirmLabel: 'Delete',
        }
      }
    );
    modalContext
      ?.onSubmit()
      .then(() => {
        ClientsService
          .deleteUmbracoManagementApiV1NoCodeDeliveryApiClientById({
            path: {
              id: client.id
            }
          })
          .then(async () => await this._loadData())
          .catch((reason) => console.error('Could not delete the client - reason: ', reason))
      })
      .catch(() => {
        // confirm dialog was cancelled
      });
  }

  static styles = css`
    .muted {
      color: var(--uui-palette-grey-dark);
    }
  `
}

declare global {
  interface HTMLElementTagNameMap {
    'no-code-delivery-api-clients-workspace-view': ClientsWorkspaceViewElement
  }
}
