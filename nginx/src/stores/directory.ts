import type { IDirectoryView } from "../models/IDirectoryView";
import { writable } from "svelte/store";
import type { Writable} from "svelte/store";

export const currentDirectory: Writable<IDirectoryView> = writable<IDirectoryView>(null);

export function fillDummyData() {
    currentDirectory.set({
      hierarchy: [
        {
          id: 1,
          name: "Home",
          isRoot: true,
          parentId: null,
        },
        {
          id: 2,
          name: "Hdawdawdaome",
          isRoot: false,
          parentId: 1,
        },
        {
          id: 3,
          name: "Homedawd dawd",
          isRoot: false,
          parentId: 2,
        }
      ]
    } as IDirectoryView);
}
