export function getIconBasedOnName(name: string): string {
    if (!name) {
        return "description";
    }
    const type = name.split(".").pop();
    if (type === name) {
        return "description";
    }

    switch (type) {
        case "pdf":
            return "picture_as_pdf";
        case "doc":
        case "docx":
        case "txt":
            return "description";
        case "xls":
        case "xlsx":
            return "table_chart";
        case "ppt":
        case "pptx":
            return "powerpoint";
        case "zip":
        case "rar":
            return "folder_zip";
        case "mp3":
            return "library_music";
        case "mp4":
            return "video_library";
        case "png":
        case "jpg":
        case "jpeg":
            return "image";
        case "gif":
            return "gif";
        case "json":
            return "data_object";
        case "js":
        case "php":
        case "html":
        case "css":
        case "scss":
        case "ts":
        case "tsx":
        case "xml":
        case "py":
        case "java":
        case "c":
        case "cpp":
        case "cs":
        case "go":
        case "h":
        case "hpp":
        case "hxx":
        case "m":
        case "mm":
        case "csproj":
        case "sln":
        case "svelte":
            return "code";
        default:
            return "description";
    }
}