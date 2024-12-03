import { LitElement, html, customElement, css, state } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { ClientsService, ClientViewModel } from "../../../api";
import { PACKAGE_NAME } from "../../../constants.ts";

@customElement('no-code-delivery-api-clients-workspace-view')
export default class ClientsWorkspaceViewElement extends UmbElementMixin(LitElement) {

  @state()
  private _serverData: Array<ClientViewModel> | undefined = undefined;

  async connectedCallback() {
    super.connectedCallback()

    const { data, error } = await ClientsService.getUmbracoManagementApiV1NoCodeDeliveryApiClient();
    if (error) {
      console.error(error);
      return;
    }

    if (data !== undefined) {
      this._serverData = data;
    }
  }

  render() {
    if (!this._serverData) {
      return html`
        <uui-box headline="${PACKAGE_NAME}">
          <uui-loader></uui-loader>
        </uui-box>
      `;
    }
    return html`
     <uui-box headline="${PACKAGE_NAME}">
       <ul>
         ${this._serverData.map(filter => html`<li>${filter.name}</li>`)}
       </ul>
      </uui-box>
    `;
  }

  static styles = css`
    uui-box {
      margin: 20px;
    }
  `
}

declare global {
  interface HTMLElementTagNameMap {
    'no-code-delivery-api-clients-workspace-view': ClientsWorkspaceViewElement
  }
}
