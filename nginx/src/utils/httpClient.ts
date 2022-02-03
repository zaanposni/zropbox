import { writable } from 'svelte/store'
import { ENABLE_CORS, API_URL } from '../config/config';
import type { IHttpClient } from '../models/IHttpClient'
import getCookie from '../utils/getCookie';

// returns a store with HTTP access functions for get, post, patch, delete
// anytime an HTTP request is made, the store is updated and all subscribers are notified.
export default function<T>(initial): IHttpClient<T> {
  // create the underlying store
  const store = writable<T>(initial) as any;

  store.upload = async (method: string, url: string, body: FormData = null, successCb: (json: any) => void = null, errorCb: (json: any) => void = null) => {
    // before we fetch, clear out previous errors and set `loading` to `true`
    store.update(data => {
        delete data.errors;
        data.loading = true;

        return data;
    });

    // define headers and body
    const headers = {
      "Content-type": "multipart/form-data",
      "Authorization": `Bearer ${getCookie('zropbox_access_token')}`
    };
    // execute fetch
    const credentials = ENABLE_CORS ? 'include' : 'same-origin';
    const response = await fetch(API_URL + url, { method, body, headers, credentials });
    // pull out json body
    const json = await response.json();

    // if response is 2xx
    if (response.ok) {
      // update the store, which will cause subscribers to be notified
      store.set({
        content: json,
        loading: false,
        errors: null
      });
      if (successCb) {
          successCb(json);
      }
    } else {
      // response failed, set `errors` and clear `loading` flag
      store.update(data => {
        data.loading = false
        data.errors = json.errors
        return data
      });
      if (errorCb) {
          errorCb(json);
      }
    }
  };

  // define a request function that will do `fetch` and update store when request finishes
  store.request = async (method: string, url: string, params: any = null, successCb: (json: any) => void = null, errorCb: (json: any) => void = null) => {
    // before we fetch, clear out previous errors and set `loading` to `true`
    store.update(data => {
      delete data.errors;
      data.loading = true;

      return data;
    });

    // define headers and body
    const headers = {
      "Content-type": "application/json",
      "Authorization": `Bearer ${getCookie('zropbox_access_token')}`
    };
    const body = params ? JSON.stringify(params) : undefined;

    // execute fetch
    const credentials = ENABLE_CORS ? 'include' : 'same-origin';
    const response = await fetch(API_URL + url, { method, body, headers, credentials });
    // pull out json body
    let json;
    try {
      json = await response.json();
    } catch(e) {
      json = {};
    }

    // if response is 2xx
    if (response.ok) {
      // update the store, which will cause subscribers to be notified
      store.set({
        content: json,
        loading: false,
        errors: null
      });
      if (successCb) {
          successCb(json);
      }
    } else {
      // response failed, set `errors` and clear `loading` flag
      store.update(data => {
        data.loading = false
        data.errors = json.errors
        return data
      });
      if (errorCb) {
          errorCb(json);
      }
    }
  }

  // convenience wrappers for get, post, patch, and delete
  store.get = (url, successCb: (json: any) => void = null, errorCb: (json: any) => void = null) => store.request('GET', url, null, successCb, errorCb);
  store.post = (url, params, successCb: (json: any) => void = null, errorCb: (json: any) => void = null) => store.request('POST', url, params, successCb, errorCb);
  store.put = (url, params, successCb: (json: any) => void = null, errorCb: (json: any) => void = null) => store.request('PUT', url, params, successCb, errorCb);
  store.delete = (url, params, successCb: (json: any) => void = null, errorCb: (json: any) => void = null) => store.request('DELETE', url, params, successCb, errorCb);

  // return the customized store
  return store;
}
