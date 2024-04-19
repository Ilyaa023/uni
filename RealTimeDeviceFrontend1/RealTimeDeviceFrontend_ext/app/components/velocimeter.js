import Component from '@ember/component';
import { inject as service} from '@ember/service';
import { computed } from '@ember/object';
import $ from 'jquery';

export default Component.extend({
    wsService: service('ws-service'),

    stateClass: computed('vm.status', function() {
        switch(this.vm.status) {
            case 'On':
                return 'on';
            case 'Off':
                return 'off';
            case 'Warning':
                return 'warn';
            case 'Emergency':
                return 'emg';
        }
    }),

    statusHeight: computed('vm.velocityValue', function() {
        return this.vm.velocity * 10;
    }),

    actions: {
        deleteVelocimeter(){
            $.ajax({
                url: `http://localhost:5000/api/Velocimeter/RemoveVelocimeter?buildingNumber=${this.vm.location.buildingNumber}&device=${this.vm.location.device}&order=${this.vm.location.order}`,
                type: 'DELETE',
                contentType:'application/json',
                success: (resp) => {
                    $.get('http://localhost:5000/api/Velocimeter/GetAllVelocimeters', (data) => {
                        this.wsService.set('velocimeters', data);
                    });
                }
            })
        },

        updateVelocimeter(){
            this.set('vm.location.buildingNumber', parseInt(this.vm.location.buildingNumber));
            this.set('vm.location.device', parseInt(this.vm.location.device));
            this.set('vm.location.order', parseInt(this.vm.location.order));
            $.ajax({
                url: `http://localhost:5000/api/Velocimeter/UpdateVelocimeter`,
                type: 'PUT',
                data: JSON.stringify(this.fd),
                contentType:'application/json',
                success: (resp) => {
                    $.get('http://localhost:5000/api/Velocimeter/GetAllVelocimeters', (data) => {
                        this.wsService.set('velocimeters', data);
                    });
                }
            })
        }
    }
});
