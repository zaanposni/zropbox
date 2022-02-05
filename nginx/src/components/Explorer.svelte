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
    import { getIconBasedOnName } from "../utils/fileIcon";
    import { shareEntryDialog, showShareEntryDialog } from "../stores/shareEntry";

    const eventDispatcher = createEventDispatcher();

    let fileInput;
    let additionalClasses = "";
    export let directoryStore: IHttpClient<ILoadingContent<IDirectoryView>>;
    export let loading: boolean = false;

    function setItemLoading(id: number, loading: boolean) {
        directoryStore.update(x => {
            let index = x?.content?.items?.findIndex(y => y.id === id);
            if (index !== null && index !== -1) {
                x.content.items[index].loading = loading;
            }
            return x;
        });
    }

    function deleteItem(id: number) {
        function deleteConfirmed(e: any) {
            if (e?.detail?.action === "yes") {
                setItemLoading(id, true);
                let deleteEntry = httpClient({});
                deleteEntry.delete(`/files/${id}`, {}, () => {
                    toastSuccess("Successfully deleted");
                    directoryStore.update(x => {
                        if (x?.content?.items) {
                            x.content.items = x.content.items.filter(y => y.id !== id);
                        }
                        return x;
                    });
                }, (error) => {
                    toastError("Failed to delete");
                    setItemLoading(id, false);
                    console.error(error);
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
        setItemLoading(item.id, true);
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
            setItemLoading(item.id, false);
            console.error(error);
        });
    }

    function shareItem(item: IDirectoryEntry) {
        if (item.loading) {
            return;
        }

        if (item.tempLink) {
            shareEntryDialog.set({
                id: 0,
                entryId: item.id,
                hash: "",
                url: item.tempLink,
                validUntil: item.tempLinkUntil
            });
            showShareEntryDialog.set(true);
        } else {
            setItemLoading(item.id, true);
            let loadTemp = httpClient({});
            loadTemp.post(`/temp/${item.id}`, {}, (response) => {
                shareEntryDialog.set({
                    id: response.id,
                    entryId: item.id,
                    hash: response.hash,
                    url: response.url,
                    validUntil: response.validUntil
                });
                showShareEntryDialog.set(true);
                setItemLoading(item.id, false);
            }, () => {
                toastError("Failed to create temporary link");
                setItemLoading(item.id, false);
            });
        }
    }

    function onDrop(e: any) {
        e.preventDefault();
        additionalClasses = "";
        if(e?.dataTransfer?.files) {
            eventDispatcher('fileSelected', {
                target: {
                    files: e.dataTransfer.files
                }
            });
        }
    }
    function onDragOver(e: any) {
        e.preventDefault();
        additionalClasses = "primary";
    }
    function onDragLeave(e: any) {
        additionalClasses = "";
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
                                <IconButton class="material-icons px-0" on:click={() => shareItem(item)} readonly={item.loading}>share</IconButton>
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
                    <div class="flex grow h-60" on:click={() => fileInput.click()}>
                        <div class="flex grow border-2 border-gray-500 border-dashed justify-center items-center cursor-pointer p-2"
                             on:drop={onDrop}
                             on:dragover={onDragOver}
                             on:dragleave={onDragLeave}>
                            <div>
                                <Icon class="material-icons {additionalClasses}" style="font-size: 100px"> upload </Icon>
                            </div>
                        </div>
                    </div>
                    <div class="invisible">
                        <input style="display:none"
                               type="file"
                               accept="*"
                               id="fileUpload"
                               on:change={(e) => eventDispatcher('fileSelected', e)}
                               bind:this={fileInput}/>
                    </div>
                {/if}
          {/if}
      </div>
    </Content>
</Card>