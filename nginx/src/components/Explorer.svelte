<script lang="ts">
    import Card, { Content } from "@smui/card";
    import IconButton, { Icon } from "@smui/icon-button";
    import { loggedInUser } from "../stores/authStore";
    import { currentDirectory } from "../stores/directory";
    import LinearProgress from '@smui/linear-progress';
    import DateDisplay from "./DateDisplay.svelte";
    import filesize from "filesize";
</script>

<Card>
    <Content class="flex flex-col p-2">
      <div class="flex flex-col">
          {#if $currentDirectory?.items}
            {#each $currentDirectory?.items as item}
                {#if item.isDir}
                    <div class="flex flex-row items-center">
                        <!-- Icon -->
                        <IconButton class="material-icons shrink-0">folder</IconButton>
                        <!-- Name -->
                        <div class="truncate" title="Foldername: {item.name}">
                            {item.name}
                        </div>
                        <div class="grow" />
                        <!-- Metadata -->
                        <div class="flex flex-row shrink-0">
                            <div title="filesize" class="greyed-text mr-2">{filesize(item.size)}</div>
                            <DateDisplay title="uploaded at" class="greyed-text" date={item.uploadedAt} />
                        </div>
                        <!-- Action buttons -->
                        <div class="flex flex-row shrink-0">
                            <IconButton class="material-icons invisible">lock</IconButton>
                            <IconButton class="material-icons invisible">share</IconButton>
                            <IconButton class="material-icons" on:click={() => { console.log("delete", item.id) }}>delete</IconButton>
                        </div>
                    </div>
                {/if}
            {/each}
            {#each $currentDirectory?.items as item}
                {#if item.isFile}
                    <div class="flex flex-row items-center">
                        <!-- Icon -->
                        <IconButton class="material-icons shrink-0">image</IconButton>
                        <!-- Name -->
                        <div class="truncate" title="Filename: {item.name}">
                            {item.name}
                        </div>
                        <div class="grow" />
                        <!-- Metadata -->
                        <div class="flex flex-row shrink-0">
                            <div title="filesize" class="greyed-text mr-2">{filesize(item.size)}</div>
                            <DateDisplay title="uploaded at" class="greyed-text" date={item.uploadedAt} />
                        </div>
                        <!-- Action buttons -->
                        <div class="flex flex-row shrink-0">
                            <IconButton toggle pressed={item.isPublic} class="material-icons px-0" on:click={() => { console.log("restrict", item.id, item.isPublic); }}>
                                <Icon class="material-icons" on>lock_open</Icon>
                                <Icon class="material-icons">lock</Icon>
                            </IconButton>
                            <IconButton class="material-icons px-0" on:click={() => { console.log("share", item.id) }}>share</IconButton>
                            <IconButton class="material-icons px-0" on:click={() => { console.log("delete", item.id) }}>delete</IconButton>
                        </div>
                    </div>
                    <LinearProgress indeterminate closed={!item.loading}/>
                {/if}
            {/each}
          {:else}
            No items found :(
          {/if}
      </div>
    </Content>
</Card>