import { manifests as entrypoints } from './entrypoints/manifest';
import { manifests as workspaces } from './workspaces/manifest';
import { ENTITY_ROOT_ALIAS, PACKAGE_NAME } from "./constants.ts";

// Job of the bundle is to collate all the manifests from different parts of the extension and load other manifests
// We load this bundle from umbraco-package.json
export const manifests: Array<UmbExtensionManifest> = [
  {
    type: 'menuItem',
    alias: 'Kjac.NoCode.DeliveryApi.MenuItem.Root',
    name: `${PACKAGE_NAME} Root Menu Item`,
    weight: 200,
    meta: {
      label: 'No-Code Delivery API',
      icon: 'icon-brackets',
      entityType: ENTITY_ROOT_ALIAS,
      menus: ['Umb.Menu.AdvancedSettings'],
    },
  },
  ...entrypoints,
  ...workspaces,
];
