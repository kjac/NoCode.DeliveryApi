(function () {
  "use strict";

  function NoCodeDeliveryApiClientsController($scope, overlayService, notificationsService, editorService, languageResource, noCodeDeliveryApiResource) {
    const vm = this;

    vm.add = add;
    vm.edit = edit;
    vm.delete = remove;

    vm.languages = [];
    languageResource.getAll().then(function (languages) {
      vm.languages = languages;
    });

    function reload() {
      vm.loading = true;
      noCodeDeliveryApiResource.client.all().then(data => {
        vm.clients = data;
        vm.loading = false;
      });
    }

    reload();

    function add() {
      open();
    }

    function edit(client) {
      open(client);
    }

    function open(client = {}) {
      const addOrEdit = !client.key ? 'Add' : 'Edit';
      const dialog = {
        title: `${addOrEdit} client`,
        client: Utilities.copy(client),
        languages: vm.languages,
        view: '/App_Plugins/NoCodeDeliveryApi/backoffice/noCodeDeliveryApi/client.html',
        size: 'small',
        submit: function () {
          editorService.close();
          reload();
        },
        close: function () {
          editorService.close();
        }
      };

      editorService.open(dialog);
    }

    function remove(client, event) {
      overlayService.open({
        title: 'Confirm delete client',
        content: 'Are you sure you want to delete the client?',
        submitButtonLabel: 'Yes, delete',
        submitButtonStyle: 'danger',
        closeButtonLabel: 'Cancel',
        submit: () => {
          noCodeDeliveryApiResource
            .client
            .delete(client.key)
            .then(() => {
              reload();
              notificationsService.success('The client was deleted.');
              overlayService.close();
            }, () => {
              notificationsService.error('Could not delete the client. Please consult the logs for more information.');
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

  angular.module('umbraco').controller('NoCodeDeliveryApi.ClientsController', NoCodeDeliveryApiClientsController);
})();
