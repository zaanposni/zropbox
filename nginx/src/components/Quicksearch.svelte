<script lang="ts">
    import { Input } from '@smui/textfield';
    import Paper from '@smui/paper';
    import Fab from '@smui/fab';
    import { Icon } from '@smui/common';
    import Card, { Content } from "@smui/card";
    import type { Writable } from 'svelte/store';
    import Explorer from './Explorer.svelte';
    import type { ILoadingContent } from '../models/ILoadingContent';
    import type { IDirectoryView } from '../models/IDirectoryView';
    import type { IHttpClient } from '../models/IHttpClient';
    import httpClient from '../utils/httpClient';

    let value = '';
    export let searchActive: Writable<boolean>;

    export const searchResults: IHttpClient<ILoadingContent<IDirectoryView>> = httpClient({});
    $: $searchActive === false ? clearSearchResult() : undefined;


    function doSearch() {
        if (value?.trim()) {
            searchActive.set(true);
            searchResults.get(`/directory/search?search=${value}`, (response) => {
                searchResults.update(x => {
                    return {
                        content: {
                            items: response,
                            currentItemId: 0,
                            hierarchy: [],
                        }
                    };
                });
            }, () => {
                searchActive.set(false);
            });
        } else {
            searchActive.set(false);
        }
    }

    function handleKeyDown(event: CustomEvent | KeyboardEvent) {
        event = event as KeyboardEvent;
        if (event.key === 'Enter') {
            doSearch();
        }
    }

    function clearSearchResult() {
        searchResults.update(x => {
            return {
                content: {
                    items: [],
                    currentItemId: 0,
                    hierarchy: [],
                }
            };
        });
    }

</script>

<style>
    .quicksearch-input-container {
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

    * :global(.quicksearch-paper > Input) {
        display: inline-block;
        margin: 0 12px;
    }

    * :global(.quicksearch-input::placeholder) {
        color: var(--mdc-theme-on-surface, #FFF);
        opacity: 0.6;
    }

    .quicksearch-results-container {
        position: relative;
        z-index: 1;
        top: 10px;
    }

    .quicksearch-results-container > div {
        position: absolute;
        z-index: 99;
    }
</style>

<div class="quicksearch-input-container">
    <Paper class="quicksearch-paper grow mr-2" elevation={6}>
        <Icon class="material-icons pl-1">search</Icon>
        <Input bind:value on:keydown={handleKeyDown} placeholder="Quicksearch" class="quicksearch-input pl-2" />
    </Paper>
    <Fab on:click={doSearch} color=primary mini class="quicksearch-fab mr-3">
        <Icon class="material-icons">arrow_forward</Icon>
    </Fab>
</div>
{#if $searchActive && $searchResults?.content?.items?.length > 0}
    <div class="quicksearch-results-container w-full">
        <div class="card-container w-full">
            <Explorer on:fileSelected
                    loading={false}
                    directoryStore={searchResults} />
            <!-- Footer -->
            <div class="greyed-text pl-2">
                {#if $searchResults?.content?.items?.length > 1}
                    {$searchResults?.content?.items?.length} items
                {/if}
                {#if $searchResults?.content?.items?.length == 1}
                    {$searchResults?.content?.items?.length} item
                {/if}
                {#if $searchResults?.content?.items?.length == 0}
                    No items found
                {/if}
            </div>
        </div>
    </div>
{/if}
{#if $searchActive && $searchResults?.content?.items?.length == 0}
    <div class="quicksearch-results-container w-full">
        <div class="card-container w-full">
            <Card>
                <Content class="flex flex-col p-2">
                    No results found :(
                </Content>
            </Card>
        </div>
    </div>
{/if}

