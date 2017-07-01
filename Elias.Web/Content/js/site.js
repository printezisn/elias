﻿// Application CSS
import '../sass/site.scss';

// Application JS
import 'bootstrap-sass';
import 'jquery-validation';
import 'jquery-validation-unobtrusive';
import 'jquery-ajax-unobtrusive';
import 'bootstrap-datepicker';
import 'fullcalendar';
import { ELApp, SCIUrlManager, SCIModal, SCIAjaxGrid, SCIForm } from './imports/_core';

window.$ = $;
window.ELApp = new ELApp();
window.SCIUrlManager = SCIUrlManager;
window.SCIModal = SCIModal;
window.SCIAjaxGrid = SCIAjaxGrid;
window.SCIForm = SCIForm;