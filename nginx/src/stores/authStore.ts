import { derived } from "svelte/store";
import type { Readable } from "svelte/store";
import type { ILoggedInUser } from "../models/ILoggedInUser";
import http from "../utils/httpClient";
import type { ILoadingContent } from "../models/ILoadingContent";
import type { IHttpClient } from "../models/IHttpClient";

export const loggedInUser: IHttpClient<ILoadingContent<ILoggedInUser>> = http<ILoadingContent<ILoggedInUser>>({});
export const isLoggedIn: Readable<boolean> = derived(loggedInUser, user => user?.content != null);
export const isAdmin: Readable<boolean> = derived(loggedInUser, user => user?.content?.isAdmin ?? false);
export const loginName: Readable<string> = derived(loggedInUser, user => user?.content?.name ?? "");
