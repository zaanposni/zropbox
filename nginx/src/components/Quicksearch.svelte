<script lang="ts">
    import { Input } from '@smui/textfield';
    import Paper from '@smui/paper';
    import Fab from '@smui/fab';
    import { Icon } from '@smui/common';
    let value = '';

    function doSearch() {
      alert('Search for ' + value);
    }

    function handleKeyDown(event: CustomEvent | KeyboardEvent) {
      event = event as KeyboardEvent;
      if (event.key === 'Enter') {
        doSearch();
      }
    }
</script>

<style>
    .quicksearch-container {
        display: flex;
        justify-content: center;
        align-items: center;
    }

    * :global(.quicksearch-paper) {
      display: flex;
      align-items: center;
      flex-grow: 1;
      height: 48px;
    }

    * :global(.quicksearch-paper > *) {
      display: inline-block;
      margin: 0 12px;
    }

    * :global(.quicksearch-input::placeholder) {
        color: var(--mdc-theme-on-surface, #FFF);
        opacity: 0.6;
    }
</style>

<div class="quicksearch-container w-100">
    <Paper class="quicksearch-paper grow mr-6" elevation={6}>
        <Icon class="material-icons">search</Icon>
        <Input
        bind:value
        on:keydown={handleKeyDown}
        placeholder="Quicksearch"
        class="quicksearch-input"
        />
    </Paper>
    <Fab
        on:click={doSearch}
        disabled={value === ''}
        color="primary"
        mini
        class="quicksearch-fab">
        <Icon class="material-icons">arrow_forward</Icon>
    </Fab>
</div>
<pre class="status">Value: {value}</pre>
