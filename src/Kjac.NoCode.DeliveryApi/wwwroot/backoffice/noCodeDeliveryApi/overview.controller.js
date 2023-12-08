(function () {
    "use strict";
console.log("Overview controller loaded")
    function NoCodeDeliveryApiOverviewController($scope, umbRequestHelper, overlayService, notificationsService, editorService, noCodeDeliveryApiResource) {
        const vm = this;

        vm.addFilter = addFilter;
        vm.editFilter = editFilter;
        vm.deleteFilter = deleteFilter;
        vm.addSort = addSort;
        vm.editSort = editSort;
        vm.deleteSort = deleteSort;

        function reload() {
            vm.loading = true;
            noCodeDeliveryApiResource.getAll().then(data => {
                vm.filters = data.filters;
                vm.sorts = data.sorts;
                vm.canAdd = (data.canAddFilter && data.canAddSort);
                vm.loading = false;
            });
        }
        
        reload();

        // ####################### FILTERS #######################

        function addFilter() {
            addItem('filter', 'filter');
        }

        function editFilter(filter) {
            editItem('filter', 'filter', filter, {});
        }
        
        function deleteFilter(filter, event) {
            deleteItem(
                'filter', 
                event, 
                () => noCodeDeliveryApiResource.deleteFilter(filter.key),
                () => {
                    const index = vm.filters.indexOf(filter);
                    vm.filters.splice(index, 1);
                });
        }

        // ####################### SORTS #######################

        function addSort(){
            addItem('sorter', 'sort');
        }

        function editSort(sort) {
            editItem('sorter', 'sort', {}, sort);
        }

        function deleteSort(sort, event) {
            deleteItem(
                'sorter',
                event,
                () => noCodeDeliveryApiResource.deleteSort(sort.key),
                () => {
                    const index = vm.sorts.indexOf(sort);
                    vm.sorts.splice(index, 1);
                });
        }
        
        // ####################### HELPERS #######################

        function addItem(itemType, viewName) {
            if(vm.canAdd === false) {
                overlayService.open({
                    title: `Cannot add ${itemType}`,
                    view: `/App_Plugins/NoCodeDeliveryApi/backoffice/noCodeDeliveryApi/rebuild.message.html`,
                    closeButtonLabel: 'Close',
                    close: () => {
                        overlayService.close();
                    }
                });

                return;
            }
            openItem(itemType, viewName);
        }

        function editItem(itemType, viewName, filter, sort){
            openItem(itemType, viewName, filter, sort);
        }

        function openItem(itemType, viewName, filter = {}, sort = {}){
            const addOrEdit = !filter.key && !sort.key ? "Add" : "Edit";
            const dialog = {
                title: `${addOrEdit} ${itemType}`,
                filter: Utilities.copy(filter),
                sort: Utilities.copy(sort),
                allFilters: vm.filters,
                allSorts: vm.sorts,
                view: `/App_Plugins/NoCodeDeliveryApi/backoffice/noCodeDeliveryApi/${viewName}.html`,
                size: "small",
                submit: function() {
                    editorService.close();
                    reload();
                },
                close: function() {
                    editorService.close();
                }
            };

            editorService.open(dialog);
        }

        function deleteItem(itemType, event, performDelete, onSuccess){
            overlayService.open({
                title: `Confirm delete ${itemType}`,
                content: `Are you sure you want to delete the ${itemType}?`,
                submitButtonLabel: 'Yes, delete',
                submitButtonStyle: 'danger',
                closeButtonLabel: 'Cancel',
                submit: () => {
                    performDelete().then(() => {
                            onSuccess();
                            notificationsService.success(`The ${itemType} was deleted.`);
                            overlayService.close();
                        }, () => {
                            notificationsService.error(`Could not delete the ${itemType}. Please consult the logs for more information.`);
                        });
                },
                close: () => {
                    overlayService.close();
                }
            });

            event.preventDefault();
            event.stopPropagation();
        }
    }

    angular.module("umbraco").controller("NoCodeDeliveryApi.OverviewController", NoCodeDeliveryApiOverviewController);
})();
