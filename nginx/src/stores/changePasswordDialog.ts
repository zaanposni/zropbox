import { writable } from "svelte/store";
import type { Writable } from "svelte/store";

export const showChangePasswordDialog: Writable<boolean> = writable(false);
export const changePasswordDialog: Writable<string> = writable<string>("");
export const changePasswordDialogReturnFunc: Writable<(e: CustomEvent<{ action: string }>) => void> = writable(null);
