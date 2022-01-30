<script lang="ts">
  import Hierarchy from "../components/Hierarchy.svelte";
  import Quicksearch from "../components/Quicksearch.svelte";
  import { fillDummyData } from "../stores/directory";

  fillDummyData();

  function changeDirectory(id: number) {
    // 0 is the users root directory
    console.log("change directory", id);
  }
</script>

<div class="flex flex-col grow items-center w-full h-full">
  <div class="w-3/5">
    <!-- Search box -->
    <div class="card-container">
      <Quicksearch />
    </div>

    <!-- Hierarchy -->
    <div class="card-container">
      <Hierarchy on:changeDir={(event) => { changeDirectory(event.detail)}} />
    </div>

    <!-- TODO: Explorer -->

    <!-- TODO: Footer -->
  </div>
</div>