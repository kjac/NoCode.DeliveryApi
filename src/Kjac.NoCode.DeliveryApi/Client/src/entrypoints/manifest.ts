export const manifests: Array<UmbExtensionManifest> = [
  {
    name: "No-Code Delivery API Entrypoint",
    alias: "Kjac.NoCode.DeliveryApi.Entrypoint",
    type: "backofficeEntryPoint",
    js: () => import("./entrypoint"),
  }
];
