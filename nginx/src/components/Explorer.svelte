<script lang="ts">
    import Card, { Content } from "@smui/card";
    import IconButton, { Icon } from "@smui/icon-button";
    import LinearProgress from '@smui/linear-progress';
    import DateDisplay from "./DateDisplay.svelte";
    import filesize from "filesize";
    import { createEventDispatcher } from "svelte";
    import { showConfirmDialog, confirmDialogReturnFunc } from '../stores/confirmDialog';
    import type { IDirectoryEntry } from "../models/IDirectoryEntry";
    import type { IHttpClient } from "../models/IHttpClient";
    import type { ILoadingContent } from "../models/ILoadingContent";
    import type { IDirectoryView } from "../models/IDirectoryView";
    import httpClient from "../utils/httpClient";
    import { toastSuccess, toastError } from "../utils/toast";

    const eventDispatcher = createEventDispatcher();

    export let directoryStore: IHttpClient<ILoadingContent<IDirectoryView>>;
    export let loading: boolean = false;

    function getIconBasedOnName(name: string): string {
        const type = name.split(".").pop();
        if (type === name) {
            return "image";
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
            default:
                return "file";
        }
    }

    function deleteItem(id: number) {
        function deleteConfirmed(e: any) {
            if (e?.detail?.action === "yes") {
                console.log("do delete", id);
                directoryStore.update(x => {
                    if (x?.content?.items) {
                        x.content.items = x.content.items.filter(y => y.id !== id);
                    }
                    return x;
                });
            }
        }
        confirmDialogReturnFunc.set(deleteConfirmed);
        showConfirmDialog.set(true);
    }

    function changePublicStatusItem(item: IDirectoryEntry) {
        if (item.loading) {
            return;
        }
        directoryStore.update(x => {
            let index = x?.content?.items?.findIndex(y => y.id === item.id);
            if (index !== null && index !== -1) {
                // x.content.items[index].isPublic = !x.content.items[index].isPublic;
                x.content.items[index].loading = true;
            }
            return x;
        });
        let updateEntry = httpClient({});
        const data = {
            name: item.name,
            isPublic: !item.isPublic
        };
        updateEntry.put(`/files/${item.id}`, data, () => {
            toastSuccess("Successfully updated");
            directoryStore.update(x => {
                let index = x?.content?.items?.findIndex(y => y.id === item.id);
                if (index !== null && index !== -1) {
                    x.content.items[index].isPublic = !x.content.items[index].isPublic;
                    x.content.items[index].loading = false;
                }
                return x;
            });
        }, (error) => {
            toastError("Failed to update");
            console.error(error);
        });
    }
</script>

<Card>
    <Content class="flex flex-col p-2">
      <div class="flex flex-col">
            {#if $directoryStore?.content?.items?.length && loading === false}
                {#each $directoryStore?.content?.items as item}
                    {#if item.isDir}
                        <div class="flex flex-row items-center">
                            <!-- Icon -->
                            <IconButton class="material-icons shrink-0" on:click={() => { eventDispatcher("changeDir", item.id); }}>folder</IconButton>
                            <div class="flex flex-row grow cursor-pointer bg-on-hover rounded-md px-2 py-1 mr-2"
                                on:click={() => { eventDispatcher("changeDir", item.id); }} title="Foldername: {item.name}">
                                <!-- Name -->
                                <div class="truncate">
                                    {item.name}
                                </div>
                                <div class="grow" />
                                <!-- Metadata -->
                                <div title="filesize" class="shrink-0 greyed-text mr-8">{ filesize(item.size) }</div>
                                <DateDisplay title="uploaded at" class="shrink-0 greyed-text" date={item.uploadedAt} />
                            </div>
                            <!-- Action buttons -->
                            <div class="flex flex-row shrink-0">
                                <IconButton class="material-icons invisible">share</IconButton>
                                <IconButton class="material-icons invisible">share</IconButton>
                                <IconButton class="material-icons px-0" on:click={() => deleteItem(item.id)}>delete</IconButton>
                            </div>
                        </div>
                        <LinearProgress indeterminate closed={!item.loading}/>
                    {/if}
                {/each}
                {#each $directoryStore?.content?.items as item}
                    {#if item.isFile}
                        <div class="flex flex-row items-center grow-0">
                            <!-- Icon -->
                            <a class="shrink-0" href="/api/files/{item.id}" target="_blank" title="Filename: {item.name}">
                                <IconButton class="material-icons">{ getIconBasedOnName(item.name) }</IconButton>
                            </a>
                            <a class="flex flex-row grow bg-on-hover rounded-md px-2 py-1 mr-2"
                                href="/api/files/{item.id}" target="_blank" title="Filename: {item.name}">
                                <!-- Name -->
                                <div class="truncate">
                                    {item.name}
                                </div>
                                <div class="grow" />
                                <!-- Metadata -->
                                <div title="filesize" class="shrink-0 greyed-text mr-8">{ filesize(item.size) }</div>
                                <DateDisplay title="uploaded at" class="shrink-0 greyed-text" date={item.uploadedAt} />
                            </a>
                            <!-- Action buttons -->
                            <div class="flex flex-row shrink-0">
                                <IconButton class="material-icons px-0" on:click={() => { console.log("share", item.id) }} readonly={item.loading}>share</IconButton>
                                <IconButton toggle pressed={item.isPublic} class="material-icons px-0" on:click={() => changePublicStatusItem(item)} readonly={item.loading}>
                                    <Icon class="material-icons" on>lock_open</Icon>
                                    <Icon class="material-icons">lock</Icon>
                                </IconButton>
                                <IconButton class="material-icons px-0" on:click={() => deleteItem(item.id)} readonly={item.loading}>delete</IconButton>
                            </div>
                        </div>
                        <LinearProgress indeterminate closed={!item.loading}/>
                    {/if}
                {/each}
            {:else}
                {#if loading}
                    <LinearProgress indeterminate />
                {:else}
                    <div>
                        <!-- TODO: add dropzone for new files -->
                        No items found :(
                    </div>
                {/if}
          {/if}
      </div>
    </Content>
</Card>