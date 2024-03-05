function NoCodeDeliveryApiResource($q, $http, umbRequestHelper) {
  function apiUrl(action) {
    return `/umbraco/backoffice/nocodedeliveryapi/${action}`;
  }

  function queryApiUrl(action) {
    return apiUrl(`queryconfiguration/${action}`);
  }

  function clientApiUrl(action) {
    return apiUrl(`clientconfiguration/${action}`);
  }

  return {
    query: {
      all() {
        return umbRequestHelper.resourcePromise(
          $http.get(queryApiUrl('all')),
          'Failed to get all filters and sorters'
        );
      },
      addFilter(filter) {
        return umbRequestHelper.resourcePromise(
          $http.post(queryApiUrl('addfilter'), filter),
          'Failed to add filter'
        );
      },
      updateFilter(filter) {
        return umbRequestHelper.resourcePromise(
          $http.put(queryApiUrl('updatefilter'), filter),
          'Failed to update filter'
        );
      },
      addSort(sort) {
        return umbRequestHelper.resourcePromise(
          $http.post(queryApiUrl('addsort'), sort),
          'Failed to add sorter'
        );
      },
      updateSort(sort) {
        return umbRequestHelper.resourcePromise(
          $http.put(queryApiUrl('updatesort'), sort),
          'Failed to update sorter'
        );
      },
      deleteFilter(key) {
        return umbRequestHelper.resourcePromise(
          $http.delete(queryApiUrl(`deletefilter?key=${key}`)),
          `Failed to delete filter with id: ${key}`
        );
      },
      deleteSort(key) {
        return umbRequestHelper.resourcePromise(
          $http.delete(queryApiUrl(`deletesort?key=${key}`)),
          `Failed to delete sorter with id: ${key}`
        );
      },
      propertyAliasPattern() {
        return '[\\w_]*';
      }
    },
    client: {
      all() {
        return umbRequestHelper.resourcePromise(
          $http.get(clientApiUrl('all')),
          'Failed to get all clients'
        );
      },
      addClient(client) {
        return umbRequestHelper.resourcePromise(
          $http.post(clientApiUrl('add'), client),
          'Failed to add client'
        );
      },
      updateClient(client) {
        return umbRequestHelper.resourcePromise(
          $http.put(clientApiUrl('update'), client),
          'Failed to update client'
        );
      },
      deleteClient(key) {
        return umbRequestHelper.resourcePromise(
          $http.delete(clientApiUrl(`delete?key=${key}`)),
          `Failed to delete client with id: ${key}`
        );
      }
    }
  };
}

angular.module('umbraco.resources').factory('noCodeDeliveryApiResource', NoCodeDeliveryApiResource);
