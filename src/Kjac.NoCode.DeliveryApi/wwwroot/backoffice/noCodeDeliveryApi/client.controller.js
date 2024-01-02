(function () {
  "use strict";

  function NoCodeDeliveryApiClientController($scope, formHelper, noCodeDeliveryApiResource) {
    const vm = this;

    vm.client = $scope.model.client;
    vm.languages = $scope.model.languages;

    vm.save = save;
    vm.close = close;

    function save() {
      if ($scope.model && $scope.model.submit && formHelper.submitForm({scope: $scope})) {
        if (vm.client.key) {
          noCodeDeliveryApiResource
            .client
            .update(vm.client)
            .then(data => {
              $scope.model.submit();
            });
        } else {
          noCodeDeliveryApiResource
            .client
            .add(vm.client)
            .then(data => {
              $scope.model.submit();
            });
        }
      }
    }

    function close() {
      if ($scope.model && $scope.model.close) {
        $scope.model.close();
      }
    }
  }

  angular.module("umbraco").controller("NoCodeDeliveryApi.ClientController", NoCodeDeliveryApiClientController);
})();
