//Permitir código Javascript en los templates
$.views.settings.allowCode(true);
$.views.helpers({
    formatDate: (val, format) => {

        return moment(val).format(format);
    }
});

$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

var container;
window.OpcionMenu = 0;
function ActualizarMenu() {
    $('#dMenu').load('Home/LoadPartialView?namepage=Shared%2F_Menu', (responseTxt, statusTxt, xhr) => {
        if (statusTxt === "error") {
            let mess = JSON.parse(responseTxt);
            Mensajes.error(mess.message);
        } else {
            let $j = jQuery.noConflict();
            $j('.easyui-linkbutton').linkbutton();
            $j('.easyui-menubutton').menubutton();
        };
    });
}

function showPopupDialog(nombrePopup, callback, callbackClose) {
    let jsonOption = {};
    jsonOption.validarData = (json) => { return true };
    jsonOption.postExec = (json) => { return true };
    jsonOption.closeEvent = (json) => { return true };

    if (typeof (callbackClose) !== 'function') {
        callbackClose = jsonOption.closeEvent;
    }

    if (nombrePopup === 'DialogContactTemplate') {
        jsonOption.title = 'Contáctanos';
        jsonOption.blockMessage = 'Enviando mensaje, espere...';
        jsonOption.url = '/Home/EnviarContactos';
    } else if (nombrePopup === 'DialogLoginTemplate') {
        jsonOption.title = 'Login';
        jsonOption.blockMessage = 'Validando, espere...';
        jsonOption.url = '/Login/Login';

        jsonOption.postExec = (json) => {
            json.message = json.message.split(',')[0];
            ActualizarMenu();

            if (typeof (callback) === "function") {
                callback();
            }

            return true
        };
    } else if (nombrePopup === 'DialogChangeUserTemplate') {
        jsonOption.title = 'Cambiar datos usuario';
        jsonOption.blockMessage = 'Validando, espere...';
        jsonOption.url = '/Home/CambiarDatos';

        setTimeout(() => {
            let usuarioName = decodeURI(getCookie('nombre'));
            $('#txtNombreChange').val(usuarioName);
        }, 1000);

        jsonOption.validarData = (json) => {
            let result = true;

            if (json.password !== json.password2) {
                result = false;
                Mensajes.error("Error", 'Las contraseñas no coinciden');
            } else {
                json.usuarioID = getCookie('usuarioID');
            }

            return result;
        };
    } else if (nombrePopup === 'DialogRegisterTemplate') {
        jsonOption.title = 'Registrarse';
        jsonOption.blockMessage = 'Registrando, espere...';
        jsonOption.url = '/Home/RegistrarUsuario';
        jsonOption.validarData = (json) => {
            let result = true;

            if (json.password !== json.password2) {
                result = false;
                Mensajes.error("Error", 'Las contraseñas no coinciden');
            } else if (!$('#txtTerminos').is(':checked')) {
                result = false;
                Mensajes.error("Error", 'Debe de aceptar los términos de uso de la plataforma');
            }

            return result;
        };
    }

    if (Object.keys(jsonOption).length > 0) {
        moduleHome.GetInstanciaDialogPopup('', nombrePopup, jsonOption);

        window.Dialog.closeEvent = callbackClose;
        window.Dialog.GetData = (data) => {
            if (jsonOption.validarData(data)) {
                showBlockUI('.ui-dialog', jsonOption.blockMessage);

                commonData.PostData(jsonOption.url, data)
                    .then((json) => {
                        if (jsonOption.postExec(json)) {
                            Mensajes.NotifySuccess(json.message);
                            window.Dialog.close(0);
                        }
                    })
                    .catch((error) => {
                        Mensajes.error("Error", error.message);

                        throw error;
                    })
                    .finally(() => {
                        showBlockUI('.ui-dialog');
                    });
            };

            return false;
        };
    }
}

function getCookie(name) {
    var match = document.cookie.match(RegExp('(?:^|;\\s*)' + name + '=([^;]*)'));
    return match ? match[1] : null;
}
function validateForm() {
    let x, y, i, valid = true;
    x = document.getElementsByClassName("step");
    y = x[currentTab].getElementsByTagName("input");

    for (i = 0; i < y.length; i++) {
        if (y[i].value == "") {
            y[i].className += " invalid";
            valid = false;
        }
    }

    if (valid) {
        document.getElementsByClassName("stepIndicator")[currentTab].className += " finish";
    }

    return valid;
}
function showBlockUI(obj, message) {
    if (message == null) {
        $(obj).unblock();
    } else {
        $(obj).block({
            message: '<h4>' + message + '</h4>',
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
        });
    }
}

