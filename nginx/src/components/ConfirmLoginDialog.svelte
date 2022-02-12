
<script lang="ts">
    import Dialog, { Title, Content, Actions } from '@smui/dialog';
    import Button, { Label } from '@smui/button';
    import IconButton from "@smui/icon-button";
    import Textfield from '@smui/textfield';
    import { confirmLoginDialog, confirmLoginDialogReturnFunc, showConfirmLoginDialog } from '../stores/confirmLoginDialog';

    let name: string = "";
    let password: string = "";
    let invalidName: boolean = false;
    let invalidPassword: boolean = false;
    $: invalidName = (name?.trim() ?? "") === "";
    $: invalidPassword = (password?.trim() ?? "") === "";

    function closeHandler(e: CustomEvent<{ action: string }>) {
        confirmLoginDialog.update(x => {
            x = {
                name,
                password
            }
            return x;
        });
        name = "";
        password = "";
        showConfirmLoginDialog.set(false);
        if ($confirmLoginDialogReturnFunc) {
            $confirmLoginDialogReturnFunc(e);
            confirmLoginDialogReturnFunc.set(null);
        }
    }
</script>

<Dialog bind:open={$showConfirmLoginDialog}
        surface$style="width: 550px; max-width: calc(100vw - 32px);"
        aria-labelledby="event-title"
        aria-describedby="event-content"
        on:SMUIDialog:closed={closeHandler}>
    <Title id="event-title">Confirm login</Title>
    <Content id="event-content">
        <div class="flex flex-col grow">
            <div class="flex flex-row items-center grow">
                <IconButton class="material-icons">badge</IconButton>
                <div class="grow">
                    <Textfield bind:value={name}
                        required
                        label="Username"
                        style="width: 100%;"
                        helperLine$style="width: 100%;">
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
        </div>
    </Content>
    <Actions>
        <Button action="cancel">
            <Label>Cancel</Label>
        </Button>
        <Button action="confirm" default disabled={invalidName || invalidPassword}>
            <Label>Confirm</Label>
        </Button>
    </Actions>
</Dialog>
