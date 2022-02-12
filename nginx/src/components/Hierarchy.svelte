<script lang="ts">
    import Card, { Content } from "@smui/card";
    import IconButton from "@smui/icon-button";
    import { createEventDispatcher } from "svelte";
    import { loggedInUser } from "../stores/authStore";
    import { currentDirectory } from "../stores/directory";
    import Menu, { MenuComponentDev } from "@smui/menu";
    import List, { Item, Text, Graphic } from "@smui/list";
    import type { IHierarchy } from "../models/IHierarchyEntry";
    import { createDirectoryDialog, createDirectoryDialogReturnFunc, showCreateDirectoryDialog } from "../stores/createDirectoryDialog";
    import httpClient from "../utils/httpClient";
    import { toastError, toastSuccess } from "../utils/toast";

    let fileinput;
    let addMenu: MenuComponentDev;
    let completePath: string = "";
    let lastThreeHierarchyElements: IHierarchy[] = [];

    $: completePath =
        "Complete path: " +
        $loggedInUser?.content?.name +
        "/" +
        $currentDirectory?.content?.hierarchy?.map((h) => h.name).join("/") ?? "";
    $: lastThreeHierarchyElements =
        $currentDirectory?.content?.hierarchy?.slice(1).slice(-3) ?? [];

    const eventDispatcher = createEventDispatcher();

    function newDirDialog() {
        createDirectoryDialogReturnFunc.set(createNewDir);
        showCreateDirectoryDialog.set(true);
    }

    function createNewDir(e) {
        if (e?.detail?.action === "upload") {
            const upload = httpClient({});
            const data = {
                name: $createDirectoryDialog,
            };
            upload.post(`/directory/${$currentDirectory.content.currentItemId}`, data, (res) => {
                currentDirectory.update(c => {
                    if (c?.content?.items) {
                        c.content.items.unshift(res);
                    }
                    return c;
                });
                toastSuccess("Directory created");
            }, () => {
                toastError("Error creating directory");
            });
        }
    }
</script>

<Card>
    <Content class="flex flex-col p-2">
        <div class="flex flex-row items-center">
            {#if $currentDirectory?.content?.hierarchy}
                <div class="flex flex-row items-center rounded-md zropbox-dropable"
                     id="0"
                     on:drop
                     on:dragover
                     on:dragleave>
                    <IconButton class="material-icons cursor-pointer"
                        on:click={() => {
                            eventDispatcher("changeDir", 0);
                        }}
                        title="return to root">folder_open</IconButton>
                    <div class="cursor-pointer bg-on-hover rounded-md"
                        on:click={() => { eventDispatcher("changeDir", 0); }}>
                        <div class="primary font-medium px-2 py-1">
                            {$loggedInUser?.content?.name}
                        </div>
                    </div>
                </div>
                <span class="font-light px-1">/</span>
                {#if $currentDirectory?.content?.hierarchy?.length < 3}
                    {#each $currentDirectory?.content?.hierarchy as directory}
                        <div class="cursor-pointer bg-on-hover rounded-md zropbox-dropable" id="{directory.id.toString()}"
                            on:click={() => { eventDispatcher("changeDir", directory.id); }}
                            on:drop
                            on:dragover
                            on:dragleave>
                            <div class="px-2 py-1">{directory.name}</div>
                        </div>
                        <span class="font-light px-1">/</span>
                    {/each}
                {:else}
                    <div class="px-2 py-1" title={completePath}>...</div>
                    <span class="font-light px-1">/</span>
                    {#each lastThreeHierarchyElements as directory}
                        <div class="cursor-pointer bg-on-hover rounded-md zropbox-dropable" id="{directory.id.toString()}"
                            on:click={() => { eventDispatcher("changeDir", directory.id); }}
                            on:drop
                            on:dragover
                            on:dragleave>
                            <div class="px-2 py-1">{directory.name}</div>
                        </div>
                        <span class="font-light px-1">/</span>
                    {/each}
                {/if}
            {:else}
                <IconButton class="material-icons cursor-pointer"
                    on:click={() => { eventDispatcher("changeDir", 0); }}
                    title="return to root">folder_open</IconButton>
                <div class="cursor-pointer bg-on-hover rounded-md"
                    on:click={() => { eventDispatcher("changeDir", 0); }}>
                    <div class="primary font-medium px-2 py-1">
                        {$loggedInUser?.content?.name}
                    </div>
                </div>
                <span class="font-light px-1">/</span>
            {/if}

            <div class="grow" />

            <div class="shrink-0">
                <IconButton class="material-icons cursor-pointer"
                    on:click={() => addMenu.setOpen(true)}>add</IconButton>
                <Menu bind:this={addMenu}>
                    <List>
                        <Item on:SMUI:action={newDirDialog}>
                            <Graphic class="material-icons mr-2">create_new_folder</Graphic>
                            <Text>Directory</Text>
                        </Item>
                        <Item on:SMUI:action={() => fileinput.click()}>
                            <Graphic class="material-icons mr-2">upload_file</Graphic>
                            <Text>File</Text>
                        </Item>
                    </List>
                </Menu>
            </div>
        </div>
    </Content>
    <div class="invisible">
        <input style="display:none"
               type="file"
               accept="*"
               on:change={(e) => eventDispatcher('fileSelected', e)}
               bind:this={fileinput}/>
    </div>
</Card>
