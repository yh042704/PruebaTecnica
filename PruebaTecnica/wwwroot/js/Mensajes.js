export class Mensajes {

    static sucess(title, mensaje) {
        Mensajes.#generalDialog('success', title, mensaje, "Aceptar", false);
    }

    static NotifySuccess(message) {
        alertify.set('notifier', 'position', 'top-right');
        alertify.success(message).dismissOthers();
    }

    static NotifyError(message) {
        //Ocultar progressbar
        if (typeof moduleHome !== "undefined") {
            moduleHome.progressBarMW.close();
        }
        
        alertify.set('notifier', 'position', 'top-right');
        alertify.error(message).dismissOthers();
    }

    static error(title, mensaje, elementFocus) {
        //Ocultar progressbar
        if (typeof moduleHome !== "undefined") {
            moduleHome.progressBarMW.close();
        }

        return Mensajes.#generalDialog('error', title, mensaje, "Aceptar", false)
            .then((result) => {
                setTimeout(function () {
                    if (elementFocus != null) {
                        elementFocus.focus();
                    }
                }, 500);

                return result;
            });
    }

    static confirm(title, mensaje, callbackYes, callbackNo, messageConfirm, messageCancel) {
        return Mensajes.#generalDialog('warning', title, mensaje,
            {
                confirm: {
                    text: messageConfirm == null ? "Aceptar" : messageConfirm,
                    value: true,
                    visible: true,
                    className: "",
                    closeModal: true
                },
                cancel: {
                    text: messageCancel == null ? "Cancelar" : messageCancel,
                    value: null,
                    visible: true,
                    className: "",
                    closeModal: true,
                }
            }, true)
            .then((value) => {
                if (value) {
                    if (callbackYes != null)
                        callbackYes();
                } else {
                    if (callbackNo != null)
                        callbackNo();
                };

                return value;
            });
    }

    static #generalDialog(icon, title, txt, buttons, danger) {
        const wrapper = document.createElement('div');
        wrapper.innerHTML = txt;

        return swal({
            icon: icon,
            title: title,
            content: wrapper,
            buttons: buttons,
            dangerMode: danger
        });
    }
}