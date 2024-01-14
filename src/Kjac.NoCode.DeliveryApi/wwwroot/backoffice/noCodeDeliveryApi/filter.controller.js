(function () {
  "use strict";

  function NoCodeDeliveryApiFilterController($scope, formHelper, noCodeDeliveryApiResource) {
    const vm = this;

    vm.filter = $scope.model.filter;
    vm.propertyAliases = vm.filter.propertyAliases
      ? vm.filter.propertyAliases.map(propertyAlias => ({propertyAlias: propertyAlias}))
      : [];
    vm.newPropertyAlias = '';
    vm.propertyAliasPattern = noCodeDeliveryApiResource.query.propertyAliasPattern();

    if (!vm.filter.filterMatchType) {
      vm.filter.filterMatchType = 'Exact';
    }

    vm.save = save;
    vm.close = close;
    vm.removePropertyAlias = removePropertyAlias;
    vm.addPropertyAlias = addPropertyAlias;
    vm.validateFilterName = validateFilterName;

    function save() {
      if ($scope.model && $scope.model.submit && formHelper.submitForm({scope: $scope})) {
        vm.filter.propertyAliases = vm.propertyAliases.map(item => item.propertyAlias);

        if (vm.filter.key) {
          noCodeDeliveryApiResource
            .query
            .updateFilter(vm.filter)
            .then(data => {
              $scope.model.submit();
            });
        } else {
          noCodeDeliveryApiResource
            .query
            .addFilter(vm.filter)
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

    function removePropertyAlias(item, evt) {
      evt.preventDefault();
      vm.propertyAliases = vm.propertyAliases.filter(i => i !== item);
    }

    function addPropertyAlias(evt) {
      evt.preventDefault();
      if (vm.newPropertyAlias) {
        vm.propertyAliases.push({propertyAlias: vm.newPropertyAlias});
        vm.newPropertyAlias = '';
      }
    }

    function validateFilterName() {
      if ($scope.filterForm) {
        const valid = vm.filter.name !== '' && !$scope.model.allFilters.find(f => f.key !== vm.filter.key && f.name.toLowerCase() === vm.filter.name.toLowerCase())
        $scope.filterForm.filterName.$setValidity('required', valid)
      }
    }
  }

  angular.module('umbraco').controller('NoCodeDeliveryApi.FilterController', NoCodeDeliveryApiFilterController);
})();
