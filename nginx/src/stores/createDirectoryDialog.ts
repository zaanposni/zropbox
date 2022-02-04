import { writable } from "svelte/store";
import type { Writable } from "svelte/store";

export const showCreateDirectoryDialog: Writable<boolean> = writable(false);
export const createDirectoryDialog: Writable<string> = writable<string>("");
export let createDirectoryDialogReturnFunc: Writable<(e: CustomEvent<{ action: string }>) => void> = writable(null);
