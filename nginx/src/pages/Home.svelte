<script lang="ts">
    import Explorer from "../components/Explorer.svelte";
    import Hierarchy from "../components/Hierarchy.svelte";
    import Quicksearch from "../components/Quicksearch.svelte";
    import { currentDirectory, fillDummyData } from "../stores/directory";

    // fillDummyData();

    function changeDirectory(id: number) {
        if (id === $currentDirectory?.content?.currentItemId || $currentDirectory?.loading === true) {
             return;
        }
        currentDirectory.get(`/directory/${id}`);
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
            <Explorer on:changeDir={(event) => { changeDirectory(event.detail)}} loading={$currentDirectory?.loading} />
        </div>

        <!-- TODO: Footer -->
        {#if $currentDirectory?.content?.items?.length}
            <div class="greyed-text pl-2">
                {$currentDirectory?.content?.items?.length} items
            </div>
        {/if}
    </div>
</div>