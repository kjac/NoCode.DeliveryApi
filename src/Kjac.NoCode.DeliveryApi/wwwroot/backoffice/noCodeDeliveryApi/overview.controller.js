(function () {
  "use strict";

  function NoCodeDeliveryApiOverviewController($scope, $timeout, $routeParams, navigationService) {
    const vm = this;

    let requestMethod = $routeParams.method;

    vm.page = {};
    vm.page.labels = {};
    vm.page.name = "";
    vm.page.navigation = [
      {
        name: 'Querying',
        icon: 'icon-filter-arrows',
        view: '/App_Plugins/noCodeDeliveryApi/backoffice/noCodeDeliveryApi/query.html',
        active: requestMethod === 'overview',
        alias: 'noCodeDeliveryApiOverview'
      },
      {
        name: 'Clients',
        icon: 'icon-sitemap',
        view: '/App_Plugins/noCodeDeliveryApi/backoffice/noCodeDeliveryApi/clients.html',
        active: requestMethod === 'client',
        alias: 'noCodeDeliveryApiClient'
      }
    ];


    // set the tree root as active
    $timeout(function () {
      navigationService.syncTree({tree: $routeParams.tree, path: [-1], activate: true});
    });
  }

  angular.module('umbraco').controller('NoCodeDeliveryApi.OverviewController', NoCodeDeliveryApiOverviewController);
})();
