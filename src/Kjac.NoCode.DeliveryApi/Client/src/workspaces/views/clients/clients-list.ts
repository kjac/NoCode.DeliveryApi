import {UmbLitElement} from '@umbraco-cms/backoffice/lit-element';
import {html, customElement, css, state, when, repeat} from '@umbraco-cms/backoffice/external/lit';
import {ClientModel} from '../../../api';
import {UMB_CONFIRM_MODAL, UMB_MODAL_MANAGER_CONTEXT} from '@umbraco-cms/backoffice/modal';
import {CLIENT_MODAL_TOKEN} from './edit-client.ts';
import {LanguageResponseModel} from '@umbraco-cms/backoffice/external/backend-api';
import {NO_CODE_DELIVERY_API_CONTEXT} from "../../workspace.context.ts";

@customElement('no-code-delivery-api-clients-workspace-view')
export default class ClientsWorkspaceViewElement extends UmbLitElement {
  #modalManagerContext?: typeof UMB_MODAL_MANAGER_CONTEXT.TYPE;
  #workspaceContext?: typeof NO_CODE_DELIVERY_API_CONTEXT.TYPE;

  @state()
  private _clients?: Array<ClientModel>;

  private _languages?: Array<LanguageResponseModel>

  constructor() {
    super();
    this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (instance) => {
      this.#modalManagerContext = instance;
    });
    this.consumeContext(NO_CODE_DELIVERY_API_CONTEXT, (instance) => {
      this.#workspaceContext = instance;
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
                      <uui-table-cell>
                        <uui-icon name="icon-unplug" aria-hidden="true"></uui-icon>
                      </uui-table-cell>
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
    const data = await this.#workspaceContext?.getClients();

    if (data) {
      this._clients = data;
    }
  }

  private _addClient = () => this._editClient();

  private async _editClient(client?: ClientModel) {
    this._languages ??= await this.#workspaceContext!.getLanguages() ?? [];

    const headline = client ? 'Edit client' : 'Add client';
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
        const success = client
          ? await this.#workspaceContext!.updateClient(
            client.id,
            {
              name: value.client.name,
              origin: value.client.origin,
              culture: value.client.culture,
              previewUrlPath: value.client.previewUrlPath,
              publishedUrlPath: value.client.publishedUrlPath
            }
          )
          : await this.#workspaceContext!.addClient(value.client);

        if (success) {
          await this._loadData();
        }
      })
      .catch(() => {
        // the modal was cancelled, do nothing
      });
  }

  private _deleteClient(client: ClientModel) {
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
      .then(async () => {
        const success = await this.#workspaceContext!.deleteClient(client.id);

        if (success) {
          await this._loadData();
        }
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
