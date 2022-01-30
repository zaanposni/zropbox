<script lang="ts">
  import Card, { Content } from "@smui/card";
  import IconButton from "@smui/icon-button";
  import { loggedInUser } from "../stores/authStore";
  import { currentDirectory, fillDummyData } from "../stores/directory";

  fillDummyData();

  function changeDirectory(id: number) {
    console.log("change directory", id);
  }
</script>

<div class="flex flex-col grow items-center w-full h-full">
  <div class="w-3/5">
    <!-- TODO: Search box -->

    <!-- Hierarchy -->
    <div class="card-container">
      <Card>
        <Content class="flex flex-col">
          {#if $currentDirectory?.hierarchy}
            <div class="flex flex-row items-center">
              <IconButton class="material-icons cursor-pointer" on:click={() => { changeDirectory(0) }}>folder_open</IconButton>
              <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { changeDirectory(0) }}>
                <div class="primary font-medium px-2 py-1">{$loggedInUser.name}</div>
              </div>
              <span class="font-light px-1">/</span>
              {#each $currentDirectory?.hierarchy as directory}
                {#if !directory.isRoot}
                  <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { changeDirectory(directory.id) }}>
                    <div class="px-2 py-1">{directory.name}</div>
                  </div>
                  <span class="font-light px-1">/</span>
                {/if}
              {/each}
            </div>
          {:else}
            <div class="flex flex-row items-center">
              <IconButton class="material-icons cursor-pointer" on:click={() => { changeDirectory(0) }}>folder_open</IconButton>
              <div class="cursor-pointer bg-on-hover rounded-md" on:click={() => { changeDirectory(0) }}>
                <div class="primary font-medium px-2 py-1">{$loggedInUser.name}</div>
              </div>
              <span class="font-light px-1">/</span>
            </div>
          {/if}
        </Content>
      </Card>
    </div>

    <!-- TODO: Explorer --> 

    <!-- TODO: Footer -->
  </div>
</div>