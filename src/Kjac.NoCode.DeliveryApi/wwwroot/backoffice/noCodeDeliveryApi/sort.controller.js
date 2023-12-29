(function () {
    "use strict";

    function NoCodeDeliveryApiSortController($scope, formHelper, noCodeDeliveryApiResource) {
        const vm = this;

        vm.sort = $scope.model.sort;
        vm.propertyAliasPattern = noCodeDeliveryApiResource.query.propertyAliasPattern();

        vm.save = save;
        vm.close = close;
        vm.validateSortName = validateSortName;

        function save() {
            if ($scope.model && $scope.model.submit && formHelper.submitForm({scope: $scope})) {
                if(vm.sort.key){
                    noCodeDeliveryApiResource
                        .query
                        .updateSort(vm.sort)
                        .then(data => {
                            $scope.model.submit();
                        });
                }
                else {
                    noCodeDeliveryApiResource
                        .query
                        .addSort(vm.sort)
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

        function validateSortName() {
            if ($scope.sortForm) {
                const valid = vm.sort.name !== '' && !$scope.model.allSorts.find(f => f.key !== vm.sort.key && f.name.toLowerCase() === vm.sort.name.toLowerCase())
                $scope.sortForm.sortName.$setValidity("required", valid)
            }
        }
    }

    angular.module("umbraco").controller("NoCodeDeliveryApi.SortController", NoCodeDeliveryApiSortController);
})();
