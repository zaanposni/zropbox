
<script lang="ts">
    import Dialog, { Title, Content, Actions } from '@smui/dialog';
    import Button, { Label } from '@smui/button';
    import { showConfirmDialog, confirmDialogReturnFunc } from '../stores/confirmDialog';

    function closeHandler(e: CustomEvent<{ action: string }>) {
        showConfirmDialog.set(false);
        if ($confirmDialogReturnFunc) {
            $confirmDialogReturnFunc(e);
            confirmDialogReturnFunc.set(null);
        }
    }
</script>

<Dialog bind:open={$showConfirmDialog}
        aria-labelledby="event-title"
        aria-describedby="event-content"
        on:SMUIDialog:closed={closeHandler}>
    <Title id="event-title">Confirmation</Title>
    <Content id="event-content">
        Are you sure?
    </Content>
    <Actions>
        <Button action="no" default>
            <Label>No</Label>
        </Button>
        <Button action="yes">
            <Label>Yes</Label>
        </Button>
    </Actions>
</Dialog>
