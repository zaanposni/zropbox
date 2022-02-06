<script lang="ts">
    import type { Writable } from "svelte/store";
    import { writable } from "svelte/store";
    import Explorer from "../components/Explorer.svelte";
    import Hierarchy from "../components/Hierarchy.svelte";
    import Quicksearch from "../components/Quicksearch.svelte";
    import { currentDirectory, fillDummyData } from "../stores/directory";
    import { uploadDialog, showUploadDialog, uploadDialogReturnFunc } from "../stores/uploadDialog";
    import httpClient from "../utils/httpClient";
    import { toastError, toastSuccess } from "../utils/toast";

    // fillDummyData();

    let searchActive: Writable<boolean> = writable(false);

    function changeDirectory(id: number) {
        if (id === $currentDirectory?.content?.currentItemId || $currentDirectory?.loading === true) {
             return;
        }
        currentDirectory.get(`/directory/${id}`);
    }

    function reload() {
        currentDirectory.get(`/directory/${$currentDirectory?.content?.currentItemId ?? 0}`);
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

    function onExitClickSearch(e) {
        if ($searchActive && !hasSomeParentTheId(e.target, "quicksearch-container")) {
            reload();
            searchActive.set(false);
        }
    }

    // returns true if the element or one of its parents has the id idname
    function hasSomeParentTheId(element, idname) {
        if (element.id === idname) return true;
        return element.parentNode && hasSomeParentTheId(element.parentNode, idname);
    }

    currentDirectory.get('/directory/0');
</script>

<style>
    .search-active > div:not(#quicksearch-container) {
        filter: blur(3px);
        opacity: 40%;
        pointer-events: none;
    }
</style>

<div class="flex flex-col grow items-center w-full h-full" on:click={onExitClickSearch}>
    <div class="w-3/5" class:search-active={$searchActive}>
        <!-- Search box -->
        <div class="card-container mb-4 dawdawdawd" id="quicksearch-container">
            <Quicksearch bind:searchActive={searchActive} />
        </div>

        <!-- Hierarchy -->
        <div class="card-container mb-4">
            <Hierarchy on:changeDir={(event) => { changeDirectory(event.detail)}}
                       on:fileSelected={onFileSelected} />
        </div>

        <!-- Explorer -->
        <div class="card-container">
            <Explorer on:changeDir={(event) => { changeDirectory(event.detail)}}
                      on:fileSelected={onFileSelected}
                      loading={$currentDirectory?.loading}
                      directoryStore={currentDirectory} />
        </div>

        <!-- Footer -->
        <div class="greyed-text pl-2">
            {#if $currentDirectory?.content?.items?.length > 1}
                {$currentDirectory?.content?.items?.length} items
            {/if}
            {#if $currentDirectory?.content?.items?.length == 1}
                {$currentDirectory?.content?.items?.length} item
            {/if}
            {#if $currentDirectory?.content?.items?.length == 0}
                No items found
            {/if}
        </div>
    </div>
</div>