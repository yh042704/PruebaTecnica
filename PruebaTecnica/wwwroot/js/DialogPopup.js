import { Mensajes } from './Mensajes.js'

export class DialogPopup {
    #dialogPop;
    #ProgressBar;
    #ProgressBarText;
    #opciones = {
        show: { effect: "blind", duration: 800 },
        position: { my: "center", at: "center", of: window }
    };
    #isProgressBar;
    #frmDataPopup;
    #isOpen = false;

    constructor(TipoDialog, template) {
        this.#dialogPop = $('#dialogTemplate').clone();
        this.#isProgressBar = TipoDialog === 'PB';

        if (this.#isProgressBar) {
            this.#dialogPop.html('<div class="button-container"><p></p><div class="progress"><div class="progress-bar bg-info progress-bar-striped progress-bar-animated" role="progressbar" style="width: 0%;text-align: center;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">0%</div></div></div>');
            this.#ProgressBar = this.#dialogPop.find('.progress-bar')[0];
            this.#ProgressBarText = this.#dialogPop.find('.button-container > p')[0];
            this.#opciones.title = "...";
            this.#ProgressBarText.textContent = 'Iniciando operación...';
            this.#opciones.dialogClass = 'no-close';
            this.#opciones.closeOnEscape = false;
        } else if (template != null) {
            let contenidoHTML = '';
            if (TipoDialog === 'HTML') {
                contenidoHTML = template;
            } else {
                let contenido = $('#' + template);
                contenidoHTML = contenido.html();
                contenidoHTML = '<form id="frm' + template + '" action="#" onkeydown="return event.keycode != 13;" novalidate>' + contenidoHTML + '</form>';

                let frmClean = $('#frm' + template);
                frmClean.parent().remove();
                frmClean.parent().empty();
            }

            this.#dialogPop.html(contenidoHTML);
            this.#frmDataPopup = this.#dialogPop.find('form');

            if (this.#frmDataPopup.length != 0) {
                this.#frmDataPopup.off();
                this.#frmDataPopup.on('submit', (event) => {
                    let form = this.#frmDataPopup;

                    event.preventDefault();
                    event.stopPropagation();

                    if (form[0].checkValidity()) {
                        let jsonData = commonData.GetSerializeObj(form);
                        if (this.GetData(jsonData)) {
                            this.close(0);
                        };
                    } else {
                        form[0].classList.add('was-validated');
                    }

                    return false;
                });
            }
        }
    }

    Open(opciones) {
        return new Promise((resolve, reject) => {
            try {
                this.#isOpen = true;

                if (!this.#isProgressBar) {
                    this.#opciones.closeOnEscape = false;
                    this.#opciones.dialogClass = "no-close";

                    if (typeof opciones === "undefined") {
                        this.#opciones.title = 'Seleccione';
                        this.#opciones.dialogClass = '';
                        //this.#opciones.closeOnEscape = true;
                        this.#opciones.buttons = [];
                        this.#opciones.width = 500;
                        this.#opciones.position = { my: "center", at: "center", of: window };
                        this.#opciones.hide = { effect: "fade", duration: 800 };
                    } else {
                        this.#opciones.title = typeof opciones.title === "undefined" ? 'Seleccione' : opciones.title;
                        this.#opciones.dialogClass = typeof opciones.dialogClass === "undefined" ? '' : opciones.dialogClass;
                        //this.#opciones.closeOnEscape = typeof opciones.closeOnEscape === "undefined" ? true : opciones.closeOnEscape;
                        this.#opciones.buttons = typeof opciones.buttons === "undefined" ? [] : opciones.buttons;
                        this.#opciones.width = typeof opciones.width === "undefined" ? 500 : opciones.width;
                        this.#opciones.position = typeof opciones.position === "undefined" ? { my: "center", at: "center", of: window } : opciones.position;
                        this.#opciones.hide = { effect: "fade", duration: 800 };
                    }

                    if (this.#opciones.buttons.length == 0) {
                        if (this.#frmDataPopup.length != 0) {
                            this.#opciones.buttons = [
                                {
                                    text: "Aceptar",
                                    click: () => {
                                        this.#frmDataPopup.submit();
                                    }
                                }
                            ];
                        };
                    };

                    this.#opciones.buttons.push(
                        {
                            text: "Cancelar",
                            click: () => {
                                this.close(0);
                            }
                        }
                    );
                }

                this.#dialogPop.dialog(this.#opciones);
                $(this).closest('[role="dialog"]').css({ opacity: '.9' });

                let buttonClose = this.#dialogPop.parent().find('.ui-dialog-titlebar-close');
                buttonClose.on("click", () => {
                    window.DialogTemplate = '';
                    this.closeEvent();
                });

                resolve(true);
            } catch (ex) {
                reject(ex);
            }
        });
    }

    close(wait) {
        if (this.#isOpen) {
            window.DialogTemplate = '';
            this.#isOpen = false;

            let dialog = this.#dialogPop;
            let waitNo = typeof wait === "undefined" ? 1000 : wait;

            setTimeout(() => {
                try {
                    dialog.dialog("close");
                    this.closeEvent();
                } catch { }
            }, waitNo);
        }
    }

    closeEvent = (data) => {
        return true;
    }

    GetData = (data) => {
        Mensajes.NotifyError('Evento sin definir!');

        return true;
    };

    async incrementarValor(valores) {
        if (!this.#isOpen) {
            await this.Open();
        }

        if (this.#isProgressBar) {
            try {
                if (typeof valores.title === 'string') {
                    this.#dialogPop.dialog({ title: valores.title });
                }

                let valeur = ~~((valores.valorActual / valores.totalRegistros) * 100);
                this.#ProgressBar.style = 'width: ' + valeur + '%';
                this.#ProgressBar.ariaValueNow = valeur;
                this.#ProgressBar.textContent = valeur + '%';
                this.#ProgressBarText.textContent = valores.textContent;

                if (valores.valorActual == valores.totalRegistros) {
                    if (typeof valores.noclose === 'undefined') {
                        this.close();
                    };
                };
            } catch (ex) {
                Mensajes.NotifyError(ex.message);
            }
        } else {
            Mensajes.NotifyError('Imposible incrementar valor debido a que el diálogo no es una barra de progreso');
        }
    }
}