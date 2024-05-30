import { commonData } from '../HTTP.js';
import { Mensajes } from '../Mensajes.js'
import { DialogPopup } from '../DialogPopup.js'

window.ShowError = (errores) => {
    let html = '<div style="height: 300px; overflow-y: scroll;">';
    $.each(errores, function (key, val) {
        html += '<blockquote class="blockquote p-4 border-left-primary-4"><p class="mb-0 p-typo">';
        Object.entries(val).map(([k, v]) => {
            html += k + ': <b>' + v + '</b><br/>';
        });
        html += '</p></blockquote>';
    });

    html += '</div>';

    let dialogError = new DialogPopup('HTML', html);
    let opciones = {
        position: { my: "center center", at: "center center", of: window },
        title: 'Errores',
        width: 600
    }

    dialogError.Open(opciones);
}

window.showSkeleton = (container) => {
    let CantSkeleton = $(window).height() > 611? 16 : 8;
    return window.LoadTemplate('#skeletonTemplate', { CantSkeleton: CantSkeleton }).then((_skeletonRender) => {
        container.html(_skeletonRender);
    }).catch((error) => {
        Mensajes.error("Error", error.message);
    });
}

window.moduleHome = {};

function Init() {
    window.loadPage = false;
    window.DialogTemplate = '';
    window.Mensajes = Mensajes;
    window.commonData = commonData;
    window.LoadTemplate = LoadTemplate;
    
    moduleHome.GetInstanciaDialogPopup = GetInstanciaDialogPopup;
    moduleHome.LoadPartialView = LoadPartialView;
    moduleHome.progressBarMW = new DialogPopup('PB');
    moduleHome.LogOut = LogOut;
    moduleHome.Utility = null;
}

function LoadPartialView(opcionMenu, pv, callback) {
    document.documentElement.scrollTop = 0;
    window.OpcionMenu = opcionMenu;

    if (window.loadPage) {
        Mensajes.NotifyError('Se está cargando una página, espere un momento...');
    } else {
        $('body').removeClass('loaded');
        window.loadPage = true;

        $('#main').load('Home/LoadPartialView?namepage=' + pv, (responseTxt, statusTxt, xhr) => {
            if (statusTxt === "error") {
                let mess = JSON.parse(responseTxt);
                Mensajes.error(mess.message);
            } 

            window.loadPage = false;

            if (callback !== null) {
                if (typeof callback === 'function') {
                    callback();
                }
            }

            $('body').addClass('loaded');
        });
    };

    return false;
}

function GetInstanciaDialogPopup(param, template, options) {
    if (window.DialogTemplate != template) {
        if (typeof (window.Dialog) !== "undefined") {
            window.Dialog.close(0);
        }

        window.DialogTemplate = template
        window.Dialog = new DialogPopup(param, template);
        window.Dialog.Open(options);
    }
}

function LoadTemplate(nameDiv, json) {
    return new Promise((resolve, reject) => {
        try {
            let divTemplate = $.templates(nameDiv);
            resolve(divTemplate.render(json));
        } catch (ex) {
            reject(ex);
        }
    });
}

function LogOut() {
    Mensajes.confirm("Cerrar sesión", "¿Desea cerrar sesión?", LogOutSi, null, "SI", "NO");
}

function LogOutSi() {
    commonData.GetData('/Login/LogOut')
        .then(() => {
            ActualizarMenu();
            LoadPartialView(2, '2,Index', loadSlider)
        })
        .catch((error) => {
            Mensajes.error(error.message);
        });
}

Init();