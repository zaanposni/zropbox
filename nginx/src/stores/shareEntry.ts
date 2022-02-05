import { writable } from "svelte/store";
import type { Writable } from "svelte/store";
import type { IShareEntry } from "../models/IShareEntry";

export const showShareEntryDialog: Writable<boolean> = writable(false);
export const shareEntryDialog: Writable<IShareEntry> = writable<IShareEntry>(null);
export const shareEntryDialogReturnFunc: Writable<(e: CustomEvent<{ action: string }>) => void> = writable(null);
