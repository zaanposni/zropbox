import { writable } from 'svelte/store';
import type { Writable } from "svelte/store";
import type { IUploadDialogEntry } from '../models/IUploadDialogEntry';

export const showUploadDialog: Writable<boolean> = writable(false);
export const uploadDialog: Writable<IUploadDialogEntry> = writable(null);
export const uploadDialogReturnFunc: Writable<(e: CustomEvent<{ action: string }>) => void> = writable(null);
