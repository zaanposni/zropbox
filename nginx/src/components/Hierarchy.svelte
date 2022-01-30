<script lang="ts">
  import Card, { Content } from "@smui/card";
  import IconButton from "@smui/icon-button";
  import { createEventDispatcher } from "svelte";
  import { loggedInUser } from "../stores/authStore";
  import { currentDirectory } from "../stores/directory";

  const eventDispatcher = createEventDispatcher();
</script>

<Card>
  <Content class="flex flex-col">
    {#if $currentDirectory?.hierarchy}
      <div class="flex flex-row items-center">
        <IconButton class="material-icons cursor-pointer" on:click={() => { eventDispatcher("changeDir", 0); }}>folder_open</IconButton>
        <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { eventDispatcher("changeDir", 0); }}>
          <div class="primary font-medium px-2 py-1">{$loggedInUser.name}</div>
        </div>
        <span class="font-light px-1">/</span>
        {#each $currentDirectory?.hierarchy as directory}
          {#if !directory.isRoot}
            <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { eventDispatcher("changeDir", directory.id); }}>
              <div class="px-2 py-1">{directory.name}</div>
            </div>
            <span class="font-light px-1">/</span>
          {/if}
        {/each}
      </div>
    {:else}
      <div class="flex flex-row items-center">
        <IconButton class="material-icons cursor-pointer" on:click={() => { eventDispatcher("changeDir", 0); }}>folder_open</IconButton>
        <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { eventDispatcher("changeDir", 0); }}>
          <div class="primary font-medium px-2 py-1">{$loggedInUser.name}</div>
        </div>
        <span class="font-light px-1">/</span>
      </div>
    {/if}
  </Content>
</Card>
