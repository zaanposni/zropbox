
<script lang="ts">
    import Dialog, { Content, Actions } from '@smui/dialog';
    import Button, { Label } from '@smui/button';
    import { shareEntryDialogReturnFunc, shareEntryDialog, showShareEntryDialog } from '../stores/shareEntry';
    import IconButton, { Icon } from "@smui/icon-button";
    import Textfield from '@smui/textfield';
    import { toastError, toastSuccess } from '../utils/toast';
    import DateDisplay from './DateDisplay.svelte';
import httpClient from '../utils/httpClient';

    function closeHandler(e: CustomEvent<{ action: string }>) {
        showShareEntryDialog.set(false);
        if ($shareEntryDialogReturnFunc) {
            $shareEntryDialogReturnFunc(e);
            shareEntryDialogReturnFunc.set(null);
        }
    }

    function copyToClipboard() {
        navigator.clipboard.writeText($shareEntryDialog.url).then(() => {
            toastSuccess('Copied to clipboard');
        }).catch(e => console.error(e));
    }

    function renew() {
       const update = httpClient({});
       update.put(`/temp/${$shareEntryDialog.entryId}`, {}, (response) => {
           toastSuccess('Renewed');
           shareEntryDialog.update(x => {
               x.validUntil = response.validUntil;
               return x;
           });
       }, () => {
           toastError('Failed to renew');
       });
    }
</script>

<Dialog bind:open={$showShareEntryDialog}
        surface$style="width: 550px; max-width: calc(100vw - 32px);"
        aria-labelledby="event-title"
        aria-describedby="event-content"
        on:SMUIDialog:closed={closeHandler}>
    <Content id="event-content">
        <div class="flex flex-col grow">
            <div class="flex flex-row items-center grow">
                <div class="grow">
                    <Textfield value={$shareEntryDialog?.url ?? ''}
                        autofocus
                        readonly={true}
                        label="Direct link"
                        style="width: 100%;"
                        helperLine$style="width: 100%;">
                    </Textfield>
                </div>
                <IconButton class="material-icons" title="copy" on:click={copyToClipboard}>content_copy</IconButton>
            </div>
            <div class="flex flex-row items-center grow">
                <div class="mr-2">Valid until:</div>
                <DateDisplay class="" title="Valid until" date={$shareEntryDialog?.validUntil}></DateDisplay>
                <IconButton class="material-icons" title="renew" on:click={renew}>refresh</IconButton>
            </div>
        </div>
    </Content>
    <Actions>
        <Button action="ok">
            <Label>Ok</Label>
        </Button>
    </Actions>
</Dialog>
