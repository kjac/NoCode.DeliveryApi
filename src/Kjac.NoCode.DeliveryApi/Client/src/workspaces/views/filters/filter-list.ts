import {UmbLitElement} from '@umbraco-cms/backoffice/lit-element';
import {html, customElement, css, repeat, when, nothing, state} from '@umbraco-cms/backoffice/external/lit';
import {FilterModel} from '../../../api';
import {UMB_CONFIRM_MODAL, UMB_MODAL_MANAGER_CONTEXT} from '@umbraco-cms/backoffice/modal';
import {FILTER_MODAL_TOKEN} from './edit-filter.ts';
import {NO_CODE_DELIVERY_API_CONTEXT_TOKEN} from "../../workspace.context.ts";

@customElement('no-code-delivery-api-filters-workspace-view')
export default class FiltersWorkspaceViewElement extends UmbLitElement {
  #modalManagerContext?: typeof UMB_MODAL_MANAGER_CONTEXT.TYPE;
  #workspaceContext?: typeof NO_CODE_DELIVERY_API_CONTEXT_TOKEN.TYPE;

  @state()
  private _filters?: Array<FilterModel>;

  private _canAddFilter: boolean = false;

  constructor() {
    super();
    this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (instance) => {
      this.#modalManagerContext = instance;
    });
    this.consumeContext(NO_CODE_DELIVERY_API_CONTEXT_TOKEN, (instance) => {
      this.#workspaceContext = instance;
    });
  }

  async connectedCallback() {
    super.connectedCallback();
    await this._loadData();
  }

  render() {
    if (!this._filters) {
      return html`
        <umb-body-layout>
          <uui-loader></uui-loader>
        </umb-body-layout>
      `;
    }

    return html`
      <umb-body-layout header-transparent>
        <uui-button look="outline" label="Add a new filter" @click=${this._addFilter}
                    disabled=${this._canAddFilter ? nothing : "true"} slot="header"></uui-button>
        <uui-box>
          ${when(
            this._filters.length > 0,
            () => html`
              <uui-table>
                <uui-table-head>
                  <uui-table-head-cell></uui-table-head-cell>
                  <uui-table-head-cell>Name</uui-table-head-cell>
                  <uui-table-head-cell>Alias</uui-table-head-cell>
                  <uui-table-head-cell>Property aliases</uui-table-head-cell>
                  <uui-table-head-cell>Match type</uui-table-head-cell>
                  <uui-table-head-cell>Field type</uui-table-head-cell>
                  <uui-table-head-cell>Index name</uui-table-head-cell>
                  <uui-table-head-cell></uui-table-head-cell>
                </uui-table-head>
                ${repeat(
                  this._filters!,
                  (filter) => filter.id,
                  (filter) => html`
                    <uui-table-row>
                      <uui-table-cell>
                        <uui-icon name="icon-filter" aria-hidden="true"></uui-icon>
                      </uui-table-cell>
                      <uui-table-cell>${filter.name}</uui-table-cell>
                      <uui-table-cell>${filter.alias}</uui-table-cell>
                      <uui-table-cell>${filter.propertyAliases.join(', ')}</uui-table-cell>
                      <uui-table-cell>${filter.filterMatchType}</uui-table-cell>
                      <uui-table-cell>${filter.primitiveFieldType}</uui-table-cell>
                      <uui-table-cell class="muted">${filter.fieldName}</uui-table-cell>
                      <uui-table-cell>
                        <uui-action-bar>
                          <uui-button label="Edit filter" look="secondary" @click=${() => this._editFilter(filter)}>
                            <uui-icon name="edit"></uui-icon>
                          </uui-button>
                          <uui-button label="Delete filter" look="secondary" @click=${() => this._deleteFilter(filter)}>
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
    const data = await this.#workspaceContext?.getFilters();

    if (data) {
      this._filters = data.filters;
      this._canAddFilter = data.canAddFilter;
    }
  }

  private _addFilter = () => this._editFilter();

  private _editFilter(filter?: FilterModel) {
    const headline = filter ? 'Edit filter' : 'Add filter';
    const modalContext = this.#modalManagerContext?.open(
      this,
      FILTER_MODAL_TOKEN,
      {
        data: {
          headline: headline,
          filter: filter,
          currentFilterNames: this._filters
            ?.filter(f => f.id !== filter?.id)
            .map(f => f.name) ?? []
        }
      }
    );
    modalContext
      ?.onSubmit()
      .then(async value => {
        const success = filter
          ? await this.#workspaceContext!.updateFilter(
            filter.id,
            {
              name: value.filter.name,
              propertyAliases: value.filter.propertyAliases
            }
          )
          : await this.#workspaceContext!.addFilter(value.filter);

        if (success) {
          await this._loadData();
        }
      })
      .catch(() => {
        // the modal was cancelled, do nothing
      });
  }

  private _deleteFilter(filter: FilterModel) {
    const modalContext = this.#modalManagerContext?.open(
      this,
      UMB_CONFIRM_MODAL,
      {
        data: {
          headline: 'Delete filter',
          content: `Are you sure you want to delete the filter "${filter.name}"?`,
          color: 'danger',
          confirmLabel: 'Delete',
        }
      }
    );
    modalContext
      ?.onSubmit()
      .then(async () => {
        const success = await this.#workspaceContext!.deleteFilter(filter.id);

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
    'no-code-delivery-api-filters-workspace-view': FiltersWorkspaceViewElement
  }
}
