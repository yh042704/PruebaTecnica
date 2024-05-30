import { Mensajes } from './Mensajes.js'

export class commonData {
    static #waitObj = null;

    static SetWaitObject(obj) {
        //obj.textillate({ loop: true, in: { effect: 'flash' }, out: { effect: 'flip' } });
        //obj.textillate();
        //commonData.#waitObj = obj;
    }

    static GetSerializeObj(obj) {
        var o = {};
        var a = obj.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });

        return o;
    }

    static GetData(URL, params) {
        let config = {
            method: 'GET',
            headers: commonData.#GetHeader,
            timeout: (3 * 60 * 1000) //3 minutos (3 * 60 * 1000)
        }

        /*        let url = new URL(URL);*/
        if (params != null) {
            URL += ('?' + new URLSearchParams(params).toString());
        }

        return commonData.#ProcCommonData(config, URL);
    }

    static GetDataBase64(url, filename, mimeType) {
        return fetch(url)
            .then(res => res.arrayBuffer())
            .then(buf => {
                return new File([buf], filename, { type: mimeType })
            })
            .catch((error) => {
                Mensajes.error("Ha ocurrido un error", error.toLocaleString());

                throw error;
            });
    }

    static GetDataBlob(pathFile) {

        return fetch(pathFile)
            .then(resp => resp.blob())
            .then(blob => {
                return blob;
            })
            .catch((error) => {
                Mensajes.error("Ha ocurrido un error", error.toLocaleString());

                throw error;
            });
    }

    static GetDataText(pathFile) {

        return fetch(pathFile)
            .then(resp => resp.text())
            .then(txt => {
                return txt;
            })
            .catch((error) => {
                Mensajes.error("Ha ocurrido un error", error.toLocaleString(), elementFocus);

                throw error;
            });
    }

    static PostData(URL, data, elementFocus) {

        return commonData.#PostDeletePutData("POST", URL, data, elementFocus);
    }

    static DeleteData(URL, data, elementFocus) {

        return commonData.#PostDeletePutData("DELETE", URL, data, elementFocus);
    }

    static PutData(URL, data, elementFocus) {

        return commonData.#PostDeletePutData("PUT", URL, data, elementFocus);
    }

    static #PostDeletePutData(method, URL, data, elementFocus) {
        let config = {
            method: method,
            headers: commonData.#GetHeader(),
            body: JSON.stringify(data)
        }

        return commonData.#ProcCommonData(config, URL, elementFocus);
    }

    static #GetToken() {
        const localstorage_user = JSON.parse(localStorage.getItem('user'))

        return localstorage_user == null ? '' : `Bearer ${localstorage_user.token}`;
    }

    static #GetHeader() {
        let Header = {
            'Content-Type': 'application/json; charset=UTF-8',
            //'Accept': 'application/json, text/plain, */*',
            'pragma': 'no-cache',
            'cache-control': 'no-cache, no-store, must-revalidate',
            "Expires": '0'
        };

        return Header;
    }

    static #ProcCommonData(method, URL, elementFocus) {
        let timeintervalid;
        let jsonResponse;
        let _jsonSucces = true;

        return fetch(URL, method)
            .then(async (response) => {
                try {
                    jsonResponse = await response.json();
                } catch (ex) {
                    if (!response.ok) throw Error(`Status code error : ${response.status}`);
                }

                try {
                    _jsonSucces = jsonResponse.success;
                } catch (ex) {
                }

                if (!_jsonSucces) {
                    throw Error(`${jsonResponse.message}`)
                }

                return jsonResponse;
            })
            .catch((error) => {
                Mensajes.error("Ha ocurrido un error", error.toLocaleString(), elementFocus);

                throw error;
            })
            .finally(() => {
                clearInterval(timeintervalid);
            });
    }
}