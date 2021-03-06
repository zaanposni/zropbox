import type { Writable } from "svelte/store";
import type { ILoadingContent } from "./ILoadingContent";

export interface IHttpClient<T> extends Writable<T> {
    upload(method: string, url: string, form: FormData, successCb: (json: any) => void, errorCb: (json: any) => void): void;
    request(method: string, url: string, params: any, successCb: (json: any) => void, errorCb: (json: any) => void): void;
    get(url: string, successCb?: (json: any) => void, errorCb?: (json: any) => void): void;
    post(url: string, params: any, successCb: (json: any) => void, errorCb: (json: any) => void): void;
    put(url: string, params: any, successCb: (json: any) => void, errorCb: (json: any) => void): void;
    delete(url: string, params: any, successCb: (json: any) => void, errorCb: (json: any) => void): void;
}
