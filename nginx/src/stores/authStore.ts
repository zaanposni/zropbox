import { writable, derived } from "svelte/store";
import type { Writable, Readable } from "svelte/store";
import type { ILoggedInUser } from "../models/ILoggedInUser";

export const loggedInUser: Writable<ILoggedInUser|null> = writable<ILoggedInUser | null>(null);
export const isLoggedIn: Readable<boolean> = derived(loggedInUser, user => user !== null);
