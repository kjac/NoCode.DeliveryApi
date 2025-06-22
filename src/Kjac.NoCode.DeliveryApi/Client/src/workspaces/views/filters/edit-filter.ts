import type {UmbModalContext, UmbModalExtensionElement} from '@umbraco-cms/backoffice/modal';
import {UmbModalToken} from '@umbraco-cms/backoffice/modal';
import {css, customElement, html, nothing, property, query, repeat, when} from '@umbraco-cms/backoffice/external/lit';
import {umbFocus, UmbLitElement} from '@umbraco-cms/backoffice/lit-element';
import {UUIInputElement} from '@umbraco-cms/backoffice/external/uui';
import {FilterMatchTypeModel, PrimitiveFieldTypeModel} from '../../../api';
import {PACKAGE_ALIAS} from '../../../constants.ts';
import {FilterBase, FilterDetails} from '../../models/filter.ts';

export type FilterModalData = {
  headline: string;
  filter?: FilterDetails;
  currentFilterNames: Array<string>
}

export type FilterModalValue = {
  filter: FilterBase;
}

export const NO_CODE_DELIVERY_API_FILTER_MODAL_TOKEN = new UmbModalToken<FilterModalData, FilterModalValue>(
  `${PACKAGE_ALIAS}.Modal.EditFilter`,
  {
    modal: {
      type: 'sidebar',
      size: 'small'
    }
  }
);

@customElement('no-code-delivery-api-edit-filter-modal-view')
export default class EditFilterModalElement
  extends UmbLitElement
  implements UmbModalExtensionElement<FilterModalData, FilterModalValue> {

  @property({attribute: false})
  modalContext?: UmbModalContext<FilterModalData, FilterModalValue>;

  @property({attribute: false})
  data?: FilterModalData;

  @property({attribute: false})
  value?: FilterModalValue;

  @query('#filterName')
  private _filterNameElement!: UUIInputElement;

  @query('#newPropertyAlias')
  private _newPropertyAliasElement!: UUIInputElement;

  @query('#submitButton')
  private _submitButtonElement!: HTMLFormElement;

  private _filter!: FilterBase;

  connectedCallback() {
    super.connectedCallback();
    this._filter = this.data?.filter ?? {
      name: '',
      propertyAliases: [],
      fieldType: PrimitiveFieldTypeModel.STRING,
      matchType: FilterMatchTypeModel.EXACT
    };
  }

  #close() {
    this.modalContext?.reject();
  }

  #submit(ev: Event) {
    ev.preventDefault();

    if (!this._filterNameElement.checkValidity() || !this._newPropertyAliasElement.checkValidity()) {
      this._submitButtonElement.state = 'failed';
      return;
    }

    this._submitButtonElement.state = 'waiting';

    this.modalContext?.updateValue({
      filter: this._filter
    });
    this.modalContext?.submit();
  }

  render() {
    return html`
      <umb-body-layout headline="${this.data?.headline}">
        <uui-form>
          <uui-box>
            <uui-label for="filterName" required>Name</uui-label>
            <small>The filter name must be unique. It is used to auto-generate the filter alias, which will be used
              for querying the Delivery API.</small>
            <umb-form-validation-message>
              <uui-input
                id="filterName"
                label="Name"
                maxlength="40"
                required
                value="${this._filter.name}"
                @change=${this._nameChanged}
                ${umbFocus()}>
              </uui-input>
            </umb-form-validation-message>

            <uui-label class="spacing-above" for="newPropertyAlias" required>Property aliases</uui-label>
            <small>The aliases of the content properties whose values should be used for filtering. The filter will be
              evaluated against any of these properties.</small>
            <umb-form-validation-message>
              <div style="display: flex;">
                <div style="flex: auto;">
                  <uui-input
                    id="newPropertyAlias"
                    label="Add new property alias"
                    required=${this._propertyAliasRequired() || nothing}>
                  </uui-input>
                </div>
                <div style="margin-left: 12px;">
                  <uui-button look="secondary" label="Add" @click=${this._addPropertyAlias}></uui-button>
                </div>
              </div>
            </umb-form-validation-message>
            ${repeat(
              this._filter.propertyAliases,
              (propertyAlias) => propertyAlias,
              (propertyAlias) => html`
                <div class="property-alias-container">
                  <uui-icon name="code" aria-hidden="true"></uui-icon>
                  <div style="flex: auto;">
                    <uui-input readonly value=${propertyAlias}>
                    </uui-input>
                  </div>
                  <div>
                    <uui-button look="outline" label="Remove" color="danger"
                                @click=${() => this._removePropertyAlias(propertyAlias)}></uui-button>
                  </div>
                </div>
              `)}

            <uui-label class="spacing-above" for="fieldType" required class="spacing-above">Field type</uui-label>
            <small>The field type determines how the filter evaluates against the content properties. String fields
              allow for exact and partial matching, while Number and Date fields can be used for exact and range
              matching.</small>
            <umb-form-validation-message>
              <uui-combobox id="fieldType"
                            required
                            readonly=${this._isEditing() ? "true" : nothing}
                            @change=${this._fieldTypeChanged}
                            value="${this._filter.fieldType}">
                <uui-combobox-list>
                  <uui-combobox-list-option value="String">String</uui-combobox-list-option>
                  <uui-combobox-list-option value="Number">Number</uui-combobox-list-option>
                  <uui-combobox-list-option value="Date">Date</uui-combobox-list-option>
                </uui-combobox-list>
              </uui-combobox>
            </umb-form-validation-message>

            ${when(
              this._filter.fieldType === 'String',
              () => html `
                <uui-label class="spacing-above" for="matchType" required>Match type</uui-label>
                <small>Use exact matching for filtering against known identifiers (e.g. content pickers or product SKUs).
                  Use partial matching for textual (wildcard) searches.</small>
                <umb-form-validation-message>
                  <uui-combobox id="matchType"
                                required
                                readonly=${this._isEditing() ? "true" : nothing}
                                @change=${this._matchTypeChanged}
                                value="${this._filter.matchType}">
                    <uui-combobox-list>
                      <uui-combobox-list-option value="Exact">Exact match</uui-combobox-list-option>
                      <uui-combobox-list-option value="Partial">Partial match</uui-combobox-list-option>
                    </uui-combobox-list>
                  </uui-combobox>
                </umb-form-validation-message>
              `
            )}
          </uui-box>
        </uui-form>

        <div slot="actions">
          <uui-button label=${this.localize.term('general_cancel')} @click=${this.#close}></uui-button>
          <uui-button
            id="submitButton"
            label=${this.localize.term('general_submit')}
            look="primary"
            color="positive"
            @click=${this.#submit}></uui-button>
        </div>
      </umb-body-layout>
    `;
  }

  private _isEditing = () => !!this.data?.filter?.id

  private _nameChanged(ev: { target: UUIInputElement }) {
    const newName = (ev.target.value as string).trim();

    // ensure name uniqueness
    if (this.data?.currentFilterNames.find(name => name.toLowerCase() === newName.toLowerCase())) {
      this._filterNameElement.setCustomValidity('Another filter with that name already exists.')
      return;
    }

    this._filterNameElement.setCustomValidity('');
    this._filter.name = newName;
  }

  private _fieldTypeChanged(ev: { target: { value: PrimitiveFieldTypeModel } }) {
    this._filter.fieldType = ev.target.value;
    if (this._filter.fieldType !== 'String') {
      this._filter.matchType = FilterMatchTypeModel.EXACT;
    }
    this.requestUpdate();
  }

  private _matchTypeChanged(ev: { target: { value: FilterMatchTypeModel } }) {
    this._filter.matchType = ev.target.value;
    this.requestUpdate();
  }

  private _addPropertyAlias() {
    const value = (this._newPropertyAliasElement.value as string)?.trim();
    if (!value) {
      return;
    }

    if (!this._filter.propertyAliases.find(alias => alias.toLowerCase() === value.toLowerCase())) {
      this._filter.propertyAliases = [...this._filter.propertyAliases, value];
      // NOTE: this._filter is not @state, need to request an update to reflect the changes
      this.requestUpdate();
    }

    this._newPropertyAliasElement.value = '';
  }

  private _removePropertyAlias(item: string) {
    this._filter.propertyAliases = this._filter.propertyAliases.filter(i => i !== item);
    // NOTE: this._filter is not @state, need to request an update to reflect the changes
    this.requestUpdate();
  }

  private _propertyAliasRequired = () => this._filter.propertyAliases.length === 0;

  static styles = css`
    uui-input {
      width: 100%;
    }

    uui-label, umb-form-validation-message, small {
      display: block;
    }

    small {
      line-height: var(--uui-size-6);
      padding-bottom: var(--uui-size-1);
    }

    .property-alias-container {
      display: flex;
      align-items: center;
      padding-left: var(--uui-size-4);
      padding-top: var(--uui-size-3);
      gap: var(--uui-size-2);
    }

    .spacing-above {
      margin-top: var(--uui-size-4);
    }
  `
}
