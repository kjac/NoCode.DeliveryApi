import {ENTITY_ROOT_ALIAS, PACKAGE_NAME, WORKSPACE_ROOT_ALIAS} from "../constants.ts";
//import { UMB_WORKSPACE_CONDITION_ALIAS } from "@umbraco-cms/backoffice/workspace";

export const manifests: Array<UmbExtensionManifest> = [
  {
    type: 'workspace',
    kind: 'default',
    alias: WORKSPACE_ROOT_ALIAS,
    name: `${PACKAGE_NAME} Root Workspace`,
    meta: {
      entityType: ENTITY_ROOT_ALIAS,
      // headline: 'My Custom Workspace',
    },
  },
  {
    type: 'workspaceView',
    alias: 'Kjac.NoCode.Delivery.Api.Workspace.Querying',
    element: () => import('./views/querying/querying-overview.ts'),
    name: `${PACKAGE_NAME} Querying Workspace View`,
    meta: {
      label: 'Querying',
      pathname: 'querying',
      icon: 'icon-filter'
    },
    conditions: [
      {
        alias: 'Umb.Condition.WorkspaceAlias', //UMB_WORKSPACE_CONDITION_ALIAS,
        match: WORKSPACE_ROOT_ALIAS,
      },
    ],
  },
  {
    type: 'workspaceView',
    alias: 'Kjac.NoCode.Delivery.Api.Workspace.Clients',
    element: () => import('./views/clients/clients-overview.ts'),
    name: `${PACKAGE_NAME} Clients Workspace View`,
    meta: {
      label: 'Clients',
      pathname: 'clients',
      icon: 'icon-unplug'
    },
    conditions: [
      {
        alias: 'Umb.Condition.WorkspaceAlias', //UMB_WORKSPACE_CONDITION_ALIAS,
        match: WORKSPACE_ROOT_ALIAS,
      },
    ],
  },
];
