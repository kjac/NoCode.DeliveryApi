import { UmbEntryPointOnInit, UmbEntryPointOnUnload } from '@umbraco-cms/backoffice/extension-api';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';
import { OpenAPI } from '../api';

export const onInit: UmbEntryPointOnInit = (_host, _extensionRegistry) => {

  // Will use only to add in Open API config with generated TS OpenAPI HTTPS Client
  _host.consumeContext(UMB_AUTH_CONTEXT, async (authContext) => {

    // Get the token info from Umbraco
    const config = authContext.getOpenApiConfiguration();

    OpenAPI.BASE = config.base;
    OpenAPI.TOKEN = () => authContext!.getLatestToken();
    OpenAPI.WITH_CREDENTIALS = true;
  });
};

export const onUnload: UmbEntryPointOnUnload = (_host, _extensionRegistry) => {
};
