
<script lang="ts">
    import Dialog, { Title, Content, Actions } from '@smui/dialog';
    import Button, { Label } from '@smui/button';
    import IconButton, { Icon } from "@smui/icon-button";
    import Textfield from '@smui/textfield';
    import CharacterCounter from '@smui/textfield/character-counter';
    import { createUserDialog, createUserDialogReturnFunc, showCreateUserDialog } from '../stores/createUserDialog';

    let name: string = "";
    let password: string = "";
    let isAdmin: boolean = false;
    let invalidName: boolean = false;
    let invalidPassword: boolean = false;
    $: invalidName = ((name?.trim() ?? "") === "") || name?.length > 30;
    $: invalidPassword = (password?.trim() ?? "") === "";

    function closeHandler(e: CustomEvent<{ action: string }>) {
        createUserDialog.update(x => {
            x = {
                name,
                password,
                isAdmin
            }
            return x;
        });
        name = "";
        password = "";
        isAdmin = false;
        showCreateUserDialog.set(false);
        if ($createUserDialogReturnFunc) {
            $createUserDialogReturnFunc(e);
            createUserDialogReturnFunc.set(null);
        }
    }
</script>

<Dialog bind:open={$showCreateUserDialog}
        surface$style="width: 550px; max-width: calc(100vw - 32px);"
        aria-labelledby="event-title"
        aria-describedby="event-content"
        on:SMUIDialog:closed={closeHandler}>
    <Title id="event-title">Create new user</Title>
    <Content id="event-content">
        <div class="flex flex-col grow">
            <div class="flex flex-row items-center grow">
                <IconButton class="material-icons">badge</IconButton>
                <div class="grow">
                    <Textfield bind:value={name}
                        required
                        label="Username"
                        input$maxlength={30}
                        style="width: 100%;"
                        helperLine$style="width: 100%;">
                        <CharacterCounter slot="helper">0 / 30</CharacterCounter>
                    </Textfield>
                </div>
            </div>
            <div class="flex flex-row items-center grow">
                <IconButton class="material-icons">password</IconButton>
                <div class="grow">
                    <Textfield bind:value={password}
                        required
                        label="Password"
                        type="password"
                        style="width: 100%;"
                        helperLine$style="width: 100%;">
                    </Textfield>
                </div>
            </div>
            <div class="flex flex-row items-center grow">
                <IconButton toggle bind:pressed={isAdmin} class="material-icons px-0">
                    <Icon class="material-icons" on>security</Icon>
                    <Icon class="material-icons">person</Icon>
                </IconButton>
                {#if isAdmin}
                    <Label class="ml-2">Admin</Label>
                {:else}
                    <Label class="ml-2">User</Label>
                {/if}
            </div>
        </div>
    </Content>
    <Actions>
        <Button action="cancel">
            <Label>Cancel</Label>
        </Button>
        <Button action="create" default disabled={invalidName || invalidPassword}>
            <Label>Create</Label>
        </Button>
    </Actions>
</Dialog>
