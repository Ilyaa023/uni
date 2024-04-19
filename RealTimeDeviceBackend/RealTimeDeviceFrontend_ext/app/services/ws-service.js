import Service from '@ember/service';
import { inject as service} from '@ember/service';

export default Service.extend({

    notifications: service('toast'),

    fireDetectors: [],

    connect() {
        let connection = new signalR.HubConnectionBuilder()
            .withUrl('http://localhost:6060/fdhub')
            .build();

        connection.on("UpdateFireDetectorEvents", data => {
            let parsedNotification = JSON.parse(data);
            parsedNotification.messages.forEach(m => {
                this.notifications.error(m, "Событие", {
                    closeButton: true,
                    debug: false,
                    newestOnTop: true,
                    progressBar: true,
                    positionClass: 'toast-top-right',
                    preventDuplicates: true,
                    onclick: null,
                    showDuration: '0',
                    hideDuration: '0',
                    timeOut: '0',
                    extendedTimeOut: '1000',
                    showEasing: 'swing',
                    hideEasing: 'linear',
                    showMethod: 'fadeIn',
                    hideMethod: 'fadeOut',
                });
            });
        });

        connection.on("UpdateFireDetectorState", data => {
            let parsedData = JSON.parse(data);
            this.set('fireDetectors', parsedData);
            console.log(parsedData);
        });

        connection.start();
    }
});
