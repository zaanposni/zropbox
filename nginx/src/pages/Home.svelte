<script lang="ts">
    import Explorer from "../components/Explorer.svelte";
    import Hierarchy from "../components/Hierarchy.svelte";
    import Quicksearch from "../components/Quicksearch.svelte";
    import { currentDirectory, fillDummyData } from "../stores/directory";
    import { uploadDialog, showUploadDialog, uploadDialogReturnFunc } from "../stores/uploadDialog";
    import httpClient from "../utils/httpClient";
    import { toastError, toastSuccess } from "../utils/toast";

    // fillDummyData();

    function changeDirectory(id: number) {
        if (id === $currentDirectory?.content?.currentItemId || $currentDirectory?.loading === true) {
             return;
        }
        currentDirectory.get(`/directory/${id}`);
    }

    const onFileSelected = (e) => {
        let image = e.detail.target.files[0];
        uploadDialog.set({
            file: image,
            name: image.name,
            size: image.size,
            isPublic: true
        });
        uploadDialogReturnFunc.set(uploadFile);
        showUploadDialog.set(true);
    };

    function uploadFile(e) {
        if (e?.detail?.action === "upload") {
            const upload = httpClient({});
            let formData = new FormData();
            formData.append("File", $uploadDialog.file);
            formData.append("Name", $uploadDialog.name);
            formData.append("IsPublic", $uploadDialog.isPublic ? 'true' : 'false');
            upload.upload("POST", `/files/${$currentDirectory.content.currentItemId}`, formData, (response) => {
                currentDirectory.update(c => {
                    toastSuccess("File uploaded");
                    if (c.content.items) {
                        c.content.items.unshift(response);
                    }
                    return c;
                })
            }, (e) => {
                if (e?.status === 413) {
                    toastError("File is too big");
                } else {
                    toastError("Error uploading file");
                }
            });
        }
    }

    currentDirectory.get('/directory/0');
</script>

<div class="flex flex-col grow items-center w-full h-full">
    <div class="w-3/5">
        <!-- Search box -->
        <div class="card-container mb-4">
            <Quicksearch />
        </div>

        <!-- Hierarchy -->
        <div class="card-container mb-4">
            <Hierarchy on:changeDir={(event) => { changeDirectory(event.detail)}} on:fileSelected={onFileSelected} />
        </div>

        <!-- Explorer -->
        <div class="card-container">
            <Explorer on:changeDir={(event) => { changeDirectory(event.detail)}} loading={$currentDirectory?.loading} directoryStore={currentDirectory} />
        </div>

        <!-- Footer -->
        {#if $currentDirectory?.content?.items?.length}
            <div class="greyed-text pl-2">
                {$currentDirectory?.content?.items?.length} items
            </div>
        {/if}
    </div>
</div>