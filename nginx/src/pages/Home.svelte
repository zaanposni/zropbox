<script lang="ts">
  import Explorer from "../components/Explorer.svelte";
  import Hierarchy from "../components/Hierarchy.svelte";
  import Quicksearch from "../components/Quicksearch.svelte";
  import { currentDirectory, fillDummyData } from "../stores/directory";

  fillDummyData();

  let explorerLoading: boolean = false;

  function changeDirectory(id: number) {
    // 0 is the users root directory
    console.log("change directory", id);
    explorerLoading = true;
    currentDirectory.update(x => {
      x.content.items = [];
      return x;
    });
  }

  currentDirectory.get('/directory/0');
</script>

<div class="flex flex-col grow items-center w-full h-full">
    <div class="w-3/5">
        <!-- Search box -->
        <div class="card-container mb-4">
            <Quicksearch />
        </div>

        <!-- Hierarchy -->
        <div class="card-container mb-4">
            <Hierarchy on:changeDir={(event) => { changeDirectory(event.detail)}} />
        </div>

        <!-- Explorer -->
        <div class="card-container">
            <Explorer on:changeDir={(event) => { changeDirectory(event.detail)}} loading={explorerLoading} />
        </div>

        <!-- TODO: Footer -->
        {#if $currentDirectory?.content?.items?.length}
            <div class="greyed-text pl-2">
                {$currentDirectory?.content?.items?.length} items
            </div>
        {/if}
    </div>
</div>