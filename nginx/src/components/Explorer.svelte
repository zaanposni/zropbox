                              <script lang="ts">
    import Card, { Content } from "@smui/card";
    import IconButton, { Icon } from "@smui/icon-button";
    import { loggedInUser } from "../stores/authStore";
    import { currentDirectory } from "../stores/directory";
    import LinearProgress from '@smui/linear-progress';
    import DateDisplay from "./DateDisplay.svelte";
    import filesize from "filesize";
    import { createEventDispatcher } from "svelte";

    const eventDispatcher = createEventDispatcher();

    export let loading: boolean = false;

    function viewItem(id: number) {
        console.log("view", id);
    }

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
</script>

<Card>
    <Content class="flex flex-col p-2">
      <div class="flex flex-col">
            {#if $currentDirectory?.content?.items && !loading}
                {#each $currentDirectory?.content?.items as item}
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
                                <div title="filesize" class="shrink-0 greyed-text mr-2">{filesize(item.size)}</div>
                                <DateDisplay title="uploaded at" class="shrink-0 greyed-text" date={item.uploadedAt} />
                            </div>
                            <!-- Action buttons -->
                            <div class="flex flex-row shrink-0">
                                <IconButton class="material-icons invisible">share</IconButton>
                                <IconButton class="material-icons invisible">share</IconButton>
                                <IconButton class="material-icons px-0" on:click={() => { console.log("delete", item.id) }}>delete</IconButton>
                            </div>
                        </div>
                        <LinearProgress indeterminate closed={!item.loading}/>
                    {/if}
                {/each}
                {#each $currentDirectory?.content?.items as item}
                    {#if item.isFile}
                        <div class="flex flex-row items-center grow-0">
                            <!-- Icon -->
                            <IconButton class="material-icons shrink-0" on:click={() => { viewItem(item.id); }}>{getIconBasedOnName(item.name)}</IconButton>
                            <div class="flex flex-row grow cursor-pointer bg-on-hover rounded-md px-2 py-1 mr-2"
                                on:click={() => { viewItem(item.id); }} title="Filename: {item.name}">
                                <!-- Name -->
                                <div class="truncate">
                                    {item.name}
                                </div>
                                <div class="grow" />
                                <!-- Metadata -->
                                <div title="filesize" class="shrink-0 greyed-text mr-2">{filesize(item.size)}</div>
                                <DateDisplay title="uploaded at" class="shrink-0 greyed-text" date={item.uploadedAt} />
                            </div>
                            <!-- Action buttons -->
                            <div class="flex flex-row shrink-0">
                                <IconButton class="material-icons px-0" on:click={() => { console.log("share", item.id) }}>share</IconButton>
                                <IconButton toggle pressed={item.isPublic} class="material-icons px-0" on:click={() => { console.log("restrict", item.id, item.isPublic); }}>
                                    <Icon class="material-icons" on>lock_open</Icon>
                                    <Icon class="material-icons">lock</Icon>
                                </IconButton>
                                <IconButton class="material-icons px-0" on:click={() => { console.log("delete", item.id) }}>delete</IconButton>
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