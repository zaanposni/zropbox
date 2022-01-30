import type { IDirectoryEntry } from "./IDirectoryEntry";
import type { IHierarchy } from "./IHierarchyEntry";

export interface IDirectoryView {
    currentItemId: number;
    items: IDirectoryEntry[];
    hierarchy: IHierarchy[];
}