
<script lang="ts">
    import Dialog, { Content, Actions } from '@smui/dialog';
    import Button, { Label } from '@smui/button';
    import { createDirectoryDialogReturnFunc, createDirectoryDialog, showCreateDirectoryDialog } from '../stores/createDirectoryDialog';
    import IconButton, { Icon } from "@smui/icon-button";
    import Textfield from '@smui/textfield';
    import CharacterCounter from '@smui/textfield/character-counter';

    let invalidName = false;
    let name = "";
    $: invalidName = ((name?.trim() ?? "") === "") || name?.length > 30;

    function closeHandler(e: CustomEvent<{ action: string }>) {
        createDirectoryDialog.update(x => {
            return name;
        });
        name = "";
        showCreateDirectoryDialog.set(false);
        if ($createDirectoryDialogReturnFunc) {
            $createDirectoryDialogReturnFunc(e);
            createDirectoryDialogReturnFunc.set(null);
        }
    }
</script>

<Dialog bind:open={$showCreateDirectoryDialog}
        surface$style="width: 550px; max-width: calc(100vw - 32px);"
        aria-labelledby="event-title"
        aria-describedby="event-content"
        on:SMUIDialog:closed={closeHandler}>
    <Content id="event-content">
        <div class="flex flex-col grow">
            <div class="flex flex-row items-center grow">
                <IconButton class="material-icons">folder_open</IconButton>
                <div class="grow">
                    <Textfield bind:value={name}
                        required
                        label="Directoryname"
                        input$maxlength={30}
                        style="width: 100%;"
                        helperLine$style="width: 100%;">
                        <CharacterCounter slot="helper">0 / 30</CharacterCounter>
                    </Textfield>
                </div>
            </div>
        </div>
    </Content>
    <Actions>
        <Button action="cancel">
            <Label>Cancel</Label>
        </Button>
        <Button action="upload" default disabled={invalidName}>
            <Label>Create</Label>
        </Button>
    </Actions>
</Dialog>
