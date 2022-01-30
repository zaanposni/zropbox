<script lang="ts">
  import Card, { Content } from "@smui/card";
  import IconButton from "@smui/icon-button";
  import { createEventDispatcher } from "svelte";
  import { loggedInUser } from "../stores/authStore";
  import { currentDirectory } from "../stores/directory";
  import Menu, { MenuComponentDev } from '@smui/menu';
  import List, { Item, Text, Graphic } from '@smui/list';
  import type { IHierarchy } from "../models/IHierarchyEntry";

  let addMenu: MenuComponentDev;
  let completePath: string = "";
  let lastThreeHierarchyElements: IHierarchy[] = [];

  $: completePath = "Complete path: " + $loggedInUser.name + "/" + $currentDirectory?.hierarchy?.filter(x => !x.isRoot)?.map(h => h.name).join("/") ?? "";
  $: lastThreeHierarchyElements = $currentDirectory?.hierarchy?.slice(1).slice(-3) ?? [];

  const eventDispatcher = createEventDispatcher();

  function createNewDir() {
    console.log("create new dir");
  }

  function uploadFile() {
    console.log("upload file");
  }
</script>

<Card>
  <Content class="flex flex-col p-2">
    <div class="flex flex-row items-center">
        {#if $currentDirectory?.hierarchy}
            <IconButton class="material-icons cursor-pointer" on:click={() => { eventDispatcher("changeDir", 0); }}>folder_open</IconButton>
            <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { eventDispatcher("changeDir", 0); }}>
              <div class="primary font-medium px-2 py-1">{$loggedInUser.name}</div>
            </div>
            <span class="font-light px-1">/</span>
            {#if $currentDirectory?.hierarchy?.length < 3}
              {#each $currentDirectory?.hierarchy as directory}
                {#if !directory.isRoot}
                    <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { eventDispatcher("changeDir", directory.id); }}>
                      <div class="px-2 py-1">{directory.name}</div>
                    </div>
                    <span class="font-light px-1">/</span>
                {/if}
              {/each}
            {:else}
              <div class="px-2 py-1" title="{completePath}">...</div>
              <span class="font-light px-1">/</span>
              {#each lastThreeHierarchyElements as directory}
                {#if !directory.isRoot}
                    <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { eventDispatcher("changeDir", directory.id); }}>
                      <div class="px-2 py-1">{directory.name}</div>
                    </div>
                    <span class="font-light px-1">/</span>
                {/if}
              {/each}
            {/if}
        {:else}
            <IconButton class="material-icons cursor-pointer" on:click={() => { eventDispatcher("changeDir", 0); }}>folder_open</IconButton>
                <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { eventDispatcher("changeDir", 0); }}>
                  <div class="primary font-medium px-2 py-1">{$loggedInUser.name}</div>
                </div>
            <span class="font-light px-1">/</span>
        {/if}

        <div class="grow"></div>

        <div class="shrink-0">
            <IconButton class="material-icons cursor-pointer" on:click={() => addMenu.setOpen(true)}>add</IconButton>
            <Menu bind:this={addMenu}>
            <List>
                <Item on:SMUI:action={createNewDir}>
                    <Graphic class="material-icons mr-2">create_new_folder</Graphic>
                    <Text>Directory</Text>
                </Item>
                <Item on:SMUI:action={uploadFile}>
                    <Graphic class="material-icons mr-2">upload_file</Graphic>
                    <Text>File</Text>
                </Item>
            </List>
            </Menu>
        </div>
    </div>
  </Content>
</Card>
