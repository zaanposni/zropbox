
<script lang="ts">
    import Dialog, { Content, Actions } from '@smui/dialog';
    import Button, { Label } from '@smui/button';
    import { changePasswordDialogReturnFunc, changePasswordDialog, showChangePasswordDialog } from '../stores/changePasswordDialog';
    import IconButton from "@smui/icon-button";
    import Textfield from '@smui/textfield';

    let invalidPassword = false;
    let password = "";
    $: invalidPassword = ((password?.trim() ?? "") === "") || password?.length > 30;

    function closeHandler(e: CustomEvent<{ action: string }>) {
        changePasswordDialog.update(x => {
            return password;
        });
        password = "";
        showChangePasswordDialog.set(false);
        if ($changePasswordDialogReturnFunc) {
            $changePasswordDialogReturnFunc(e);
            changePasswordDialogReturnFunc.set(null);
        }
    }

    const onKeyPress = e => {
        if (e.charCode === 13 && !invalidPassword) {
            closeHandler({
                detail: {
                    action: "update"
                }
            } as any);
        };
    };
</script>

<Dialog bind:open={$showChangePasswordDialog}
        surface$style="width: 550px; max-width: calc(100vw - 32px);"
        aria-labelledby="event-title"
        aria-describedby="event-content"
        on:SMUIDialog:closed={closeHandler}>
    <Content id="event-content">
        <div class="flex flex-col grow">
            <div class="flex flex-row items-center grow">
                <IconButton class="material-icons">lock</IconButton>
                <div class="grow" on:keypress={onKeyPress}>
                    <Textfield bind:value={password}
                        required
                        type="password"
                        label="New password"
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
        <Button action="update" default disabled={invalidPassword}>
            <Label>Update</Label>
        </Button>
    </Actions>
</Dialog>
