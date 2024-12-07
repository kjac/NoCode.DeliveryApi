import {UmbLitElement} from '@umbraco-cms/backoffice/lit-element';
import {html, customElement, state, nothing, when, repeat, css} from '@umbraco-cms/backoffice/external/lit';
import {SortersService, SortModel} from '../../../api';
import {UMB_CONFIRM_MODAL, UMB_MODAL_MANAGER_CONTEXT} from '@umbraco-cms/backoffice/modal';
import {SORTER_MODAL_TOKEN} from './edit-sorter.ts';
import {tryExecuteAndNotify} from '@umbraco-cms/backoffice/resources';

@customElement('no-code-delivery-api-sorters-workspace-view')
export default class SortersWorkspaceViewElement extends UmbLitElement {
  #modalManagerContext?: typeof UMB_MODAL_MANAGER_CONTEXT.TYPE;

  @state()
  private _sorters?: Array<SortModel>;

  private _canAddSorter: boolean = false;

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
    if (!this._sorters) {
      return html`
        <umb-body-layout>
          <uui-loader></uui-loader>
        </umb-body-layout>
      `;
    }
    return html`
      <umb-body-layout header-transparent>
        <uui-button look="outline" label="Add a new sorter" @click=${this._addSorter}
                    disabled=${this._canAddSorter ? nothing : "true"} slot="header"></uui-button>
        <uui-box>
          ${when(
            this._sorters.length > 0,
            () => html`
              <uui-table>
                <uui-table-head>
                  <uui-table-head-cell></uui-table-head-cell>
                  <uui-table-head-cell>Name</uui-table-head-cell>
                  <uui-table-head-cell>Alias</uui-table-head-cell>
                  <uui-table-head-cell>Property alias</uui-table-head-cell>
                  <uui-table-head-cell>Field type</uui-table-head-cell>
                  <uui-table-head-cell>Index name</uui-table-head-cell>
                  <uui-table-head-cell></uui-table-head-cell>
                </uui-table-head>
                ${repeat(
                  this._sorters!,
                  (sorter) => sorter.id,
                  (sorter) => html`
                    <uui-table-row>
                      <uui-table-cell>
                        <uui-icon name="icon-filter" aria-hidden="true"></uui-icon>
                      </uui-table-cell>
                      <uui-table-cell>${sorter.name}</uui-table-cell>
                      <uui-table-cell>${sorter.alias}</uui-table-cell>
                      <uui-table-cell>${sorter.propertyAlias}</uui-table-cell>
                      <uui-table-cell>${sorter.primitiveFieldType}</uui-table-cell>
                      <uui-table-cell class="muted">${sorter.fieldName}</uui-table-cell>
                      <uui-table-cell>
                        <uui-action-bar>
                          <uui-button label="Edit filter" look="secondary" @click=${() => this._editSorter(sorter)}>
                            <uui-icon name="edit"></uui-icon>
                          </uui-button>
                          <uui-button label="Delete filter" look="secondary" @click=${() => this._deleteSorter(sorter)}>
                            <uui-icon name="delete"></uui-icon>
                          </uui-button>
                        </uui-action-bar>
                      </uui-table-cell>
                    </uui-table-row>
                  `)}
              </uui-table>
            `,
            () => html`<p>No filters have been added yet.</p>`
          )}
        </uui-box>
      </umb-body-layout>
    `;
  }

  private async _loadData() {
    const {data} = await tryExecuteAndNotify(this, SortersService.getNoCodeDeliveryApiSort());
    if (data) {
      this._sorters = data.sorts;
      this._canAddSorter = data.canAddSort;
    }
  }

  private _addSorter = () => this._editSorter();

  private _editSorter(sorter?: SortModel) {
    const headline = sorter ? 'Edit sorter' : 'Add sorter';
    const modalContext = this.#modalManagerContext?.open(
      this,
      SORTER_MODAL_TOKEN,
      {
        data: {
          headline: headline,
          sorter: sorter,
          currentSorterNames: this._sorters?.map(sorter => sorter.name) ?? []
        }
      }
    );
    modalContext
      ?.onSubmit()
      .then(async value => {
        const {error} = await tryExecuteAndNotify(
          this,
          sorter
            ? SortersService.putNoCodeDeliveryApiSortById({
              id: sorter.id,
              requestBody: {
                name: value.sorter.name,
                propertyAlias: value.sorter.propertyAlias
              }
            })
            : SortersService.postNoCodeDeliveryApiSort({requestBody: value.sorter})
        );

        if (!error) {
          await this._loadData();
        }
      })
      .catch(() => {
        // the modal was cancelled, do nothing
      });
  }

  private _deleteSorter(sorter: SortModel) {
    const modalContext = this.#modalManagerContext?.open(
      this,
      UMB_CONFIRM_MODAL,
      {
        data: {
          headline: 'Delete sorter',
          content: `Are you sure you want to delete the sorter "${sorter.name}"?`,
          color: 'danger',
          confirmLabel: 'Delete',
        }
      }
    );
    modalContext
      ?.onSubmit()
      .then(async () => {
        const {error} = await tryExecuteAndNotify(
          this,
          SortersService.deleteNoCodeDeliveryApiSortById({id: sorter.id})
        );

        if (!error) {
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
    'no-code-delivery-api-sorters-workspace-view': SortersWorkspaceViewElement
  }
}
