import Route from '@ember/routing/route';
import { inject as service} from '@ember/service';
import $ from 'jquery';

export default Route.extend({
    wsService: service('ws-service'),

    model() {
        return $.get('http://localhost:5000/api/FireDetector/GetAllFireDetectors', (data) => {
            this.wsService.set('fireDetectors', data);
        });
    },

    setupController(controller){
        this.wsService.connect();
        controller.set('ws', this.wsService);
    }
});
