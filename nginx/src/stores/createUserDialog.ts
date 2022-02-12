import { writable } from 'svelte/store';
import type { Writable } from "svelte/store";
import type { INewUser } from '../models/INewUser';

export const showCreateUserDialog: Writable<boolean> = writable(false);
export const createUserDialog: Writable<INewUser> = writable(null);
export const createUserDialogReturnFunc: Writable<(e: CustomEvent<{ action: string }>) => void> = writable(null);
