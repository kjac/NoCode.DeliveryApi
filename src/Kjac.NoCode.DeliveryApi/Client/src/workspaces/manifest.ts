import {ENTITY_ROOT_ALIAS, PACKAGE_ALIAS, PACKAGE_NAME, WORKSPACE_ROOT_ALIAS} from "../constants.ts";
//import { UMB_WORKSPACE_CONDITION_ALIAS } from "@umbraco-cms/backoffice/workspace";

export const manifests: Array<UmbExtensionManifest> = [
  {
    type: 'workspace',
    kind: 'default',
    alias: WORKSPACE_ROOT_ALIAS,
    name: `${PACKAGE_NAME} Root Workspace`,
    meta: {
      entityType: ENTITY_ROOT_ALIAS,
      // TODO: include workspace headline when the umbraco package supports it
      // headline: `${PACKAGE_NAME}`,
      // TODO VERIFY: figure out how to remove the workspace footer
    },
  },
  {
    type: 'workspaceView',
    alias: `${PACKAGE_ALIAS}.Workspace.Filters`,
    element: () => import('./views/filters/filter-list.ts'),
    name: `${PACKAGE_NAME} Filter List Workspace View`,
    meta: {
      label: 'Filters',
      pathname: 'filters',
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
    alias: `${PACKAGE_ALIAS}.Workspace.Sorters`,
    element: () => import('./views/sorters/sorter-list.ts'),
    name: `${PACKAGE_NAME} Sorter List Workspace View`,
    meta: {
      label: 'Sorters',
      pathname: 'sorters',
      icon: 'icon-filter-arrows'
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
    alias: `${PACKAGE_ALIAS}.Workspace.Clients`,
    element: () => import('./views/clients/clients-list.ts'),
    name: `${PACKAGE_NAME} Clients List Workspace View`,
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
  {
    type: 'modal',
    alias: `${PACKAGE_ALIAS}.Modal.EditFilter`,
    name: `${PACKAGE_NAME} Edit Filter Modal View`,
    element: () => import('./views/filters/edit-filter.ts')
  },
  {
    type: 'modal',
    alias: `${PACKAGE_ALIAS}.Modal.EditSorter`,
    name: `${PACKAGE_NAME} Edit Sorter Modal View`,
    element: () => import('./views/sorters/edit-sorter.ts')
  }
];
