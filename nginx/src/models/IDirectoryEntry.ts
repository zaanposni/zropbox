export interface IDirectoryEntry {
    id: number;
    name: string;
    isDir: boolean;
    isFile: boolean;
    isRoot: boolean;
    tempLink?: string;
    tempLinkUntil?: Date;
    size: number;
    uploadedAt: Date;
    uploadedBy: string;
    isPublic: boolean;
    loading?: boolean;
}
