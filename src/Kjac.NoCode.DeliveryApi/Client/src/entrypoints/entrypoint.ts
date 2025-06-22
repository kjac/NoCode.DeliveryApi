import {UmbEntryPointOnInit, UmbEntryPointOnUnload} from '@umbraco-cms/backoffice/extension-api';
import {UMB_AUTH_CONTEXT} from '@umbraco-cms/backoffice/auth';
import {client} from '../api/client.gen.js';

export const onInit: UmbEntryPointOnInit = (_host, _extensionRegistry) => {
  // Will use only to add in Open API config with generated TS OpenAPI HTTPS Client
  // Do the OAuth token handshake stuff
  _host.consumeContext(UMB_AUTH_CONTEXT, async (authContext) => {
    // Get the token info from Umbraco
    const config = authContext?.getOpenApiConfiguration();

    client.setConfig({
      auth: config?.token ?? undefined,
      baseUrl: config?.base ?? "",
      credentials: config?.credentials ?? "same-origin",
    });
  });
};

export const onUnload: UmbEntryPointOnUnload = (_host, _extensionRegistry) => {
};
