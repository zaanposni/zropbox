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
        },
        {
          id: 3,
          name: "Homedawd dawd",
          isRoot: false,
          parentId: 2,
        },
        {
          id: 3,
          name: "Homedawd dawd",
          isRoot: false,
          parentId: 2,
        },
        {
          id: 3,
          name: "Homedawd dawd",
          isRoot: false,
          parentId: 2,
        },
        {
          id: 3,
          name: "Homedawd dawd",
          isRoot: false,
          parentId: 2,
        },
        {
          id: 3,
          name: "Homedawd dawd",
          isRoot: false,
          parentId: 2,
        },
        {
          id: 3,
          name: "Homedawd dawd",
          isRoot: false,
          parentId: 2,
        }
      ],
      items: [
        {
          id: 15,
          name: "test.png",
          isDir: false,
          isFile: true,
          isRoot: false,
          tempLink: null,
          tempLinkUntil: null,
          size: 123123,
          uploadedAt: new Date(),
          uploadedBy: "test",
          isPublic: false,
        },
        {
          id: 16,
          name: "test_dawdadawdawd -awd-a-daw-d-ad-a.txt",
          isDir: false,
          isFile: true,
          isRoot: false,
          tempLink: null,
          tempLinkUntil: null,
          size: 123,
          uploadedAt: new Date(),
          uploadedBy: "test",
          isPublic: true,
        },
        {
          id: 17,
          name: "testdawdaw.pdf",
          isDir: false,
          isFile: true,
          isRoot: false,
          tempLink: null,
          tempLinkUntil: null,
          size: 31237819,
          uploadedAt: new Date(),
          uploadedBy: "test",
          isPublic: false,
        },
        {
          id: 18,
          name: "wdawdda idhjoaw indoiawn doiawn wwwwwww",
          isDir: true,
          isFile: false,
          isRoot: false,
          tempLink: null,
          tempLinkUntil: null,
          size: 0,
          uploadedAt: new Date(),
          uploadedBy: "test",
          isPublic: false,
        }
      ]
    } as IDirectoryView);
}
