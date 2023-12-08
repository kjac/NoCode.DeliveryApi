function NoCodedeliveryapiResource($q, $http, umbRequestHelper) {
    function apiUrl(action) {
        return`/umbraco/backoffice/nocodedeliveryapi/configuration/${action}`;
    }

    return {
        getAll() {
            return umbRequestHelper.resourcePromise(
                $http.get(apiUrl('getall')),
                'Failed to get all filters and sorters'
            );
        },
        addFilter(filter) {
            return umbRequestHelper.resourcePromise(
                $http.post(apiUrl('addfilter'), filter),
                `Failed to add filter`
            );
        },
        updateFilter(filter) {
            return umbRequestHelper.resourcePromise(
                $http.put(apiUrl('updatefilter'), filter),
                `Failed to update filter`
            );
        },
        addSort(sort) {
            return umbRequestHelper.resourcePromise(
                $http.post(apiUrl('addsort'), sort),
                `Failed to add sorter`
            );
        },
        updateSort(sort) {
            return umbRequestHelper.resourcePromise(
                $http.put(apiUrl('updatesort'), sort),
                `Failed to update sorter`
            );
        },
        deleteFilter(key) {
            return umbRequestHelper.resourcePromise(
                $http.delete(apiUrl(`deletefilter?key=${key}`)),
                `Failed to delete filter with id: ${key}`
            );
        },
        deleteSort(key) {
            return umbRequestHelper.resourcePromise(
                $http.delete(apiUrl(`deletesort?key=${key}`)),
                `Failed to delete sorter with id: ${key}`
            );
        },
        propertyAliasPattern() {
            return '[\\w_]*';
        }
    };
}
angular.module('umbraco.resources').factory('noCodeDeliveryApiResource', NoCodedeliveryapiResource);
