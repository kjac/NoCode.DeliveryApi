import { LitElement, html, customElement, css, state } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { ClientsService, ClientViewModel } from "../../../api";

@customElement('no-code-delivery-api-clients-workspace-view')
export default class ClientsWorkspaceViewElement extends UmbElementMixin(LitElement) {

  @state()
  private _clients: Array<ClientViewModel> | undefined = undefined;

  async connectedCallback() {
    super.connectedCallback()

    const { data, error } = await ClientsService.getUmbracoManagementApiV1NoCodeDeliveryApiClient();
    if (error) {
      console.error(error);
      return;
    }

    this._clients = data;
  }

  render() {
    if (!this._clients) {
      return html`
        <uui-box>
          <uui-loader></uui-loader>
        </uui-box>
      `;
    }
    return html`
     <uui-box>
       <ul>
         ${this._clients.map(filter => html`<li>${filter.name}</li>`)}
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
