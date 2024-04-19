import Route from '@ember/routing/route';
import { inject as service } from '@ember/service';
import $ from 'jquery';

export default Route.extend({
    wsService: service('ws-service'),

    

    model() {
        return $.get('http://localhost:5000/api/Velocimeter/GetAllVelocimeters', (data) => {
            this.wsService.set('velocimeters', data);
        });
    },

    setupController(controller) {
        this.wsService.connect();
        controller.set('ws', this.wsService);
        controller.set('newVelocimeter', {location: {}});
    },

    actions: {
        addVelocimeter(newVelocimeter, controller) {
            newVelocimeter.location.buildingNumber = parseInt(newVelocimeter.location.buildingNumber);
            newVelocimeter.location.device = parseInt(newVelocimeter.location.device);
            newVelocimeter.location.order = parseInt(newVelocimeter.location.order);
            $.ajax({
                url: `http://localhost:5000/api/Velocimeter/addVelocimeter`,
                type: 'POST',
                data: JSON.stringify(newVelocimeter),
                contentType: 'application/json',
                success: (resp) => {
                    $.get('http://localhost:5000/api/Velocimeter/GetAllVelocimeters', (data) => {
                        this.wsService.set('velocimeters', data);
                        controller.set('newVelocimeter', {location: {}});
                    });
                }
            })
        }
    }
});
