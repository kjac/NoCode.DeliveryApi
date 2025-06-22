import type {UmbModalContext, UmbModalExtensionElement} from '@umbraco-cms/backoffice/modal';
import {UmbModalToken} from '@umbraco-cms/backoffice/modal';
import {css, customElement, html, nothing, property, query} from '@umbraco-cms/backoffice/external/lit';
import {umbFocus, UmbLitElement} from '@umbraco-cms/backoffice/lit-element';
import {UUIInputElement} from '@umbraco-cms/backoffice/external/uui';
import {PrimitiveFieldTypeModel} from '../../../api';
import {PACKAGE_ALIAS} from '../../../constants.ts';
import {SorterBase, SorterDetails} from '../../models/sorter.ts';

export type SorterModalData = {
  headline: string;
  sorter?: SorterDetails;
  currentSorterNames: Array<string>
}

export type SorterModalValue = {
  sorter: SorterBase;
}

export const NO_CODE_DELIVERY_API_SORTER_MODAL_TOKEN = new UmbModalToken<SorterModalData, SorterModalValue>(
  `${PACKAGE_ALIAS}.Modal.EditSorter`,
  {
    modal: {
      type: 'sidebar',
      size: 'small'
    }
  }
);

@customElement('no-code-delivery-api-edit-sorter-modal-view')
export default class EditSorterModalElement
  extends UmbLitElement
  implements UmbModalExtensionElement<SorterModalData, SorterModalValue> {

  @property({attribute: false})
  modalContext?: UmbModalContext<SorterModalData, SorterModalValue>;

  @property({attribute: false})
  data?: SorterModalData;

  @property({attribute: false})
  value?: SorterModalValue;

  @query('#sorterName')
  private _sorterNameElement!: UUIInputElement;

  @query('#propertyAlias')
  private _propertyAliasElement!: UUIInputElement;

  @query('#submitButton')
  private _submitButtonElement!: HTMLFormElement;

  private _sorter!: SorterBase;

  connectedCallback() {
    super.connectedCallback();
    this._sorter = this.data?.sorter ?? {
      name: '',
      fieldType: PrimitiveFieldTypeModel.STRING,
      propertyAlias: ''
    };
  }

  #close() {
    this.modalContext?.reject();
  }

  #submit(ev: Event) {
    ev.preventDefault();

    if (!this._sorterNameElement.checkValidity() || !this._propertyAliasElement.checkValidity()) {
      this._submitButtonElement.state = 'failed';
      return;
    }

    this._submitButtonElement.state = 'waiting';

    this.modalContext?.updateValue({
      sorter: this._sorter
    });
    this.modalContext?.submit();
  }

  render() {
    return html`
      <umb-body-layout headline="${this.data?.headline}">
        <uui-form>
          <uui-box>
            <uui-label for="sorterName" required>Name</uui-label>
            <small>The sorter name must be unique. It is used to auto-generate the sorter alias, which will be used
              for querying the Delivery API.</small>
            <umb-form-validation-message>
              <uui-input
                id="sorterName"
                label="Name"
                maxlength="40"
                required
                value="${this._sorter.name}"
                @change=${this._nameChanged}
                ${umbFocus()}>
              </uui-input>
            </umb-form-validation-message>

            <uui-label for="propertyAlias" required class="spacing-above">Property alias</uui-label>
            <small>The alias of the content property whose value should be used for sorting.</small>
            <umb-form-validation-message>
              <uui-input
                id="propertyAlias"
                label="Property alias"
                required
                value="${this._sorter.propertyAlias}"
                @change=${(e: { target: { value: string; }; }) => this._sorter.propertyAlias = e.target.value}>
              </uui-input>
            </umb-form-validation-message>

            <uui-label class="spacing-above" for="fieldType" required class="spacing-above">Field type</uui-label>
            <small>
              The field type determines how the sorting works:
              <ul>
                <li>A Number field performs correct sorting of numeric values</li>
                <li>A Date field ensures correct sorting of date values.</li>
              </ul>
              Use a String field to perform alphanumeric sorting, if the property value is not expected to be a number
              or a date.
            </small>
            <umb-form-validation-message>
              <uui-combobox id="fieldType"
                            required
                            readonly=${this._isEditing() ? "true" : nothing}
                            @change=${(e: { target: { value: PrimitiveFieldTypeModel; }; }) => this._sorter.fieldType = e.target.value}
                            value="${this._sorter.fieldType}">
                <uui-combobox-list>
                  <uui-combobox-list-option value="String">String</uui-combobox-list-option>
                  <uui-combobox-list-option value="Number">Number</uui-combobox-list-option>
                  <uui-combobox-list-option value="Date">Date</uui-combobox-list-option>
                </uui-combobox-list>
              </uui-combobox>
            </umb-form-validation-message>
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

  private _isEditing = () => !!this.data?.sorter?.id

  private _nameChanged(ev: Event) {
    const newName = ((ev.target as UUIInputElement).value as string).trim();

    // ensure name uniqueness
    if (this.data?.currentSorterNames.find(name => name.toLowerCase() === newName.toLowerCase())) {
      this._sorterNameElement.setCustomValidity('Another sorter with that name already exists.')
      return;
    }

    this._sorterNameElement.setCustomValidity('');
    this._sorter.name = newName;
  }

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

    .spacing-above {
      margin-top: var(--uui-size-4);
    }
  `
}
