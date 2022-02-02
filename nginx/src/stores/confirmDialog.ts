import { writable } from "svelte/store";
import type { Writable } from "svelte/store";

export const showConfirmDialog: Writable<boolean> = writable(false);
export let confirmDialogReturnFunc: Writable<(e: CustomEvent<{ action: string }>) => void> = writable(null);
