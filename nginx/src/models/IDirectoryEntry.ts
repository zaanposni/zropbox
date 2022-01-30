export interface IDirectoryEntry {
    id: number;
    name: string;
    isDirectory: boolean;
    isFile: boolean;
    isRoot: boolean;
    tempLink: string;
    tempLinkUntil: Date;
    size: number;
    uploadedAt: Date;
    uploadedBy: string;
    isPublic: boolean;
}
