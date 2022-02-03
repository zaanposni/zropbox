<script lang="ts">
    import Card, { Content } from "@smui/card";
    import IconButton from "@smui/icon-button";
    import { createEventDispatcher } from "svelte";
    import { loggedInUser } from "../stores/authStore";
    import { currentDirectory } from "../stores/directory";
    import Menu, { MenuComponentDev } from "@smui/menu";
    import List, { Item, Text, Graphic } from "@smui/list";
    import type { IHierarchy } from "../models/IHierarchyEntry";
    import { uploadDialog, showUploadDialog, uploadDialogReturnFunc } from "../stores/uploadDialog";
    import httpClient from "../utils/httpClient";
    import { toastError, toastSuccess } from "../utils/toast";

    let fileinput;

    const onFileSelected = (e) => {
        let image = e.target.files[0];
        uploadDialog.set({
            file: image,
            name: image.name,
            size: image.size,
            isPublic: true
        });
        uploadDialogReturnFunc.set(uploadFile);
        showUploadDialog.set(true);
    };

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

    function createNewDir() {
        console.log("create new dir");
    }

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
</script>

<Card>
    <Content class="flex flex-col p-2">
        <div class="flex flex-row items-center">
            {#if $currentDirectory?.content?.hierarchy}
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
                <span class="font-light px-1">/</span>
                {#if $currentDirectory?.content?.hierarchy?.length < 3}
                    {#each $currentDirectory?.content?.hierarchy as directory}
                        <div class="cursor-pointer bg-on-hover rounded-md"
                            on:click={() => { eventDispatcher("changeDir", directory.id); }}>
                            <div class="px-2 py-1">{directory.name}</div>
                        </div>
                        <span class="font-light px-1">/</span>
                    {/each}
                {:else}
                    <div class="px-2 py-1" title={completePath}>...</div>
                    <span class="font-light px-1">/</span>
                    {#each lastThreeHierarchyElements as directory}
                        <div class="cursor-pointer bg-on-hover rounded-md"
                            on:click={() => { eventDispatcher("changeDir", directory.id); }}>
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
                        <Item on:SMUI:action={createNewDir}>
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
               on:change={(e) => onFileSelected(e)}
               bind:this={fileinput}/>
    </div>
</Card>
