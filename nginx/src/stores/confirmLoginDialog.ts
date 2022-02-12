import { writable } from 'svelte/store';
import type { Writable } from "svelte/store";
import type { IConfirmLogin } from '../models/IConfirmLogin';

export const showConfirmLoginDialog: Writable<boolean> = writable(false);
export const confirmLoginDialog: Writable<IConfirmLogin> = writable(null);
export const confirmLoginDialogReturnFunc: Writable<(e: CustomEvent<{ action: string }>) => void> = writable(null);
