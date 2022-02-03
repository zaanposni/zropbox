
<script lang="ts">
    import Dialog, { Content, Actions } from '@smui/dialog';
    import Button, { Label } from '@smui/button';
    import { showUploadDialog, uploadDialogReturnFunc, uploadDialog } from '../stores/uploadDialog';
    import { getIconBasedOnName } from "../utils/fileIcon";
    import IconButton, { Icon } from "@smui/icon-button";
    import fileSize from 'filesize';
    import Textfield from '@smui/textfield';
    import CharacterCounter from '@smui/textfield/character-counter';

    let invalidFilename = false;
    let name = "";
    let icon = "image";
    let size = "";
    let isPublic = false;
    $: invalidFilename = ((name?.trim() ?? "") === "") || name?.length > 30;
    $: $uploadDialog?.name ? loadInitial() : undefined;
    $: icon = getIconBasedOnName(name);
    $: size = fileSize($uploadDialog?.file?.size ?? 0, { fullform: true});

    function loadInitial() {
        name = $uploadDialog?.name ?? "";
        if (name?.length > 30) {
            name = "";
        }
        invalidFilename = ((name?.trim() ?? "") === "") || name?.length > 30;
    }

    function closeHandler(e: CustomEvent<{ action: string }>) {
        uploadDialog.update(x => {
            x.name = name;
            x.isPublic = isPublic;
            return x;
        });
        showUploadDialog.set(false);
        if ($uploadDialogReturnFunc) {
            $uploadDialogReturnFunc(e);
            uploadDialogReturnFunc.set(null);
        }
    }
</script>

<Dialog bind:open={$showUploadDialog}
        surface$style="width: 550px; max-width: calc(100vw - 32px);"
        aria-labelledby="event-title"
        aria-describedby="event-content"
        on:SMUIDialog:closed={closeHandler}>
    <Content id="event-content">
        <div class="flex flex-col grow">
            <div class="flex flex-row items-center grow">
                <IconButton class="material-icons">{icon}</IconButton>
                <div class="grow">
                    <Textfield bind:value={name}
                        required
                        label="Filename"
                        input$maxlength={30}
                        style="width: 100%;"
                        helperLine$style="width: 100%;">
                        <CharacterCounter slot="helper">0 / 30</CharacterCounter>
                    </Textfield>
                </div>
            </div>
            <div class="flex flex-row items-center">
                <IconButton toggle bind:pressed={isPublic} class="material-icons px-0">
                    <Icon class="material-icons" on>lock_open</Icon>
                    <Icon class="material-icons">lock</Icon>
                </IconButton>
                {#if isPublic}
                    <Label class="ml-2">Public</Label>
                {:else}
                    <Label class="ml-2">Private</Label>
                {/if}
            </div>
            <div class="flex flex-row items-center">
                <IconButton class="material-icons" on>save</IconButton>
                <Label class="ml-2">{size}</Label>
            </div>
        </div>
    </Content>
    <Actions>
        <Button action="cancel">
            <Label>Cancel</Label>
        </Button>
        <Button action="upload" default disabled={invalidFilename}>
            <Label>Upload</Label>
        </Button>
    </Actions>
</Dialog>
