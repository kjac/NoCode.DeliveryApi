import {UmbModalToken} from '@umbraco-cms/backoffice/modal';
import {html, property, customElement, css, nothing, query, repeat, when} from '@umbraco-cms/backoffice/external/lit';
import {umbFocus, UmbLitElement} from '@umbraco-cms/backoffice/lit-element';
import type {UmbModalContext, UmbModalExtensionElement} from '@umbraco-cms/backoffice/modal';
import {PACKAGE_ALIAS} from '../../../constants.ts';
import {LanguageResponseModel} from '@umbraco-cms/backoffice/external/backend-api';
import {UUIInputElement} from '@umbraco-cms/backoffice/external/uui';
import {ClientBase, ClientDetails} from '../../models/client.ts';

export type ClientModalData = {
  headline: string;
  client?: ClientDetails;
  languages: Array<LanguageResponseModel>
}

export type ClientModalValue = {
  client: ClientBase;
}

export const NO_CODE_DELIVERY_API_CLIENT_MODAL_TOKEN = new UmbModalToken<ClientModalData, ClientModalValue>(
  `${PACKAGE_ALIAS}.Modal.EditClient`,
  {
    modal: {
      type: 'sidebar',
      size: 'small'
    }
  }
);

@customElement('no-code-delivery-api-edit-client-modal-view')
export default class EditClientModalElement
  extends UmbLitElement
  implements UmbModalExtensionElement<ClientModalData, ClientModalValue> {

  @property({attribute: false})
  modalContext?: UmbModalContext<ClientModalData, ClientModalValue>;

  @property({attribute: false})
  data?: ClientModalData;

  @property({attribute: false})
  value?: ClientModalValue;

  @query('#form')
  private _formElement!: HTMLFormElement;

  @query('#submitButton')
  private _submitButtonElement!: HTMLFormElement;

  private _client!: ClientBase;

  connectedCallback() {
    super.connectedCallback();
    this._client = this.data?.client ?? {
      name: '',
      origin: ''
    };
  }

  #close() {
    this.modalContext?.reject();
  }

  #submit() {
    this._formElement.requestSubmit();
  }

  // TODO: prevent enter from submitting the modal --- how? it works for core modals such as add/edit property
  #onFormSubmit(ev: Event) {
    ev.preventDefault();

    if (!this._formElement.checkValidity()) {
      this._submitButtonElement.state = 'failed';
      return;
    }

    this._submitButtonElement.state = 'waiting';

    this.modalContext?.updateValue({
      client: this._client
    });
    this.modalContext?.submit();
  }

  render() {
    return html`
      <umb-body-layout headline="${this.data?.headline}">
        <uui-form>
          <form name="form" id="form" @submit=${this.#onFormSubmit}>
            <uui-box>
              <uui-label for="clientName" required>Name</uui-label>
              <small>The client pretty-name. This is only used for identification purposes.</small>
              <umb-form-validation-message>
                <uui-input
                  id="clientName"
                  label="Name"
                  maxlength="40"
                  required
                  value="${this._client.name}"
                  @change=${(e: { target: { value: string; }; }) => this._client.name = e.target.value}>
                  ${umbFocus()}>
                </uui-input>
              </umb-form-validation-message>

              <uui-label for="propertyAlias" required class="spacing-above">Origin</uui-label>
              <small>The origin of the client. This will be used to apply CORS policies and build outgoing links (if
                configured below). It must be a well-formed URL including the HTTP scheme, e.g. https://mydomain.com or
                http://localhost:3000.</small>
              <umb-form-validation-message>
                <uui-input
                  id="clientOrigin"
                  label="Origin"
                  type="url"
                  maxlength="200"
                  required
                  value="${this._client.origin}"
                  @change=${(e: { target: { value: string; }; }) => this._client.origin = e.target.value}>
                </uui-input>
              </umb-form-validation-message>

              <uui-label class="spacing-above" for="previewUrlPath" class="spacing-above">Preview path</uui-label>
              <small>
                If the client supports preview, enter the path that triggers preview. The path will be appended to the
                origin.<br/>
                Various placeholders can be applied to provide context. See the docs for more info.
              </small>
              <uui-input
                id="previewUrlPath"
                label="Preview path"
                placeholder="/trigger-preview?id={id}"
                maxlength="200"
                value="${this._client.previewUrlPath}"
                @change=${this._previewUrlPathChanged}>
              </uui-input>

              <uui-label class="spacing-above" for="publishedUrlPath" class="spacing-above">Preview path</uui-label>
              <small>
                To link directly from the document editor "Info" section to the client, enter the path to use for the
                link. The path will be appended to the origin.<br/>
                Various placeholders can be applied to provide context. See the docs for more info.
              </small>
              <uui-input
                id="publishedUrlPath"
                label="Published path"
                placeholder="/{culture}/{path}"
                maxlength="200"
                value="${this._client.publishedUrlPath}"
                @change=${this._publishedUrlPathChanged}>
              </uui-input>

              ${when(
                this.data!.languages.length > 1,
                () => html`
                  <uui-label class="spacing-above" for="culture" class="spacing-above">Language</uui-label>
                  <small>If the client only handles a specific language (culture), select that language here. This
                    ensures that preview and/or published links are only available when editing content in the selected
                    language.</small>
                  <uui-combobox id="culture"
                                disabled=${this._client.previewUrlPath || this._client.publishedUrlPath ? nothing : "true"}
                                @change=${(e: { target: { value: string; }; }) => this._client.culture = e.target.value}
                                value="${this._client.culture}">
                    <uui-combobox-list>
                      ${repeat(
                        this.data!.languages,
                        (language) => language.isoCode,
                        (language) => html`
                          <uui-combobox-list-option value="${language.isoCode}">${language.name}</uui-combobox-list-option>
                        `
                      )}
                    </uui-combobox-list>
                  </uui-combobox>
                `
              )}
            </uui-box>

          </form>
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

  private _previewUrlPathChanged(ev: Event) {
    this._client.previewUrlPath = ((ev.target as UUIInputElement).value as string).trim();
    this.requestUpdate();
  }

  private _publishedUrlPathChanged(ev: Event) {
    this._client.publishedUrlPath = ((ev.target as UUIInputElement).value as string).trim();
    this.requestUpdate();
  }

  static styles = css`
    uui-input, uui-label, umb-form-validation-message, small {
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
