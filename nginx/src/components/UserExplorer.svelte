<script lang="ts">
    import Card, { Content } from "@smui/card";
    import type { ILoadingContent } from "../models/ILoadingContent";
    import type { IUser } from "../models/IUser";
    import httpClient from "../utils/httpClient";
    import IconButton, { Icon } from "@smui/icon-button";
    import LinearProgress from '@smui/linear-progress';
    import { loginName } from "../stores/authStore";
    import { toastError, toastSuccess } from "../utils/toast";
    import { createUserDialog, createUserDialogReturnFunc, showCreateUserDialog } from "../stores/createUserDialog";
    import Fab from '@smui/fab';
    import { changePasswordDialog, changePasswordDialogReturnFunc, showChangePasswordDialog } from "../stores/changePasswordDialog";
import { confirmDialogReturnFunc, showConfirmDialog } from "../stores/confirmDialog";

    let selectedUser: IUser | null = null;
    const allUsers = httpClient<ILoadingContent<IUser[]>>({});

    function setItemLoading(id: number, loading: boolean) {
        allUsers.update(x => {
            let index = x?.content?.findIndex(y => y.id === id);
            if (index !== null && index !== -1) {
                x.content[index].loading = loading;
            }
            return x;
        });
    }

    function changeAdminStatus(user: IUser) {
        if (user.loading) {
            return;
        }
        setItemLoading(user.id, true);
        let updateEntry = httpClient({});
        const data = {
            username: user.name,
            isAdmin: !user.isAdmin,
            password: "."
        };
        updateEntry.put(`/users/admin`, data, () => {
            toastSuccess("Successfully updated");
            allUsers.update(x => {
                let index = x?.content?.findIndex(y => y.id === user.id);
                if (index !== null && index !== -1) {
                    x.content[index].isAdmin = !x.content[index].isAdmin;
                    x.content[index].loading = false;
                }
                return x;
            });
        }, (error) => {
            toastError("Failed to update");
            setItemLoading(user.id, false);
            console.error(error);
        });
    }

    function deleteUser(user: IUser) {
        function deleteConfirmed(e: any) {
            if (e?.detail?.action === "yes") {
                setItemLoading(user.id, true);
                let deleteEntry = httpClient({});
                deleteEntry.delete(`/users/${user.name}`, {}, () => {
                    toastSuccess("Successfully deleted");
                    allUsers.update(x => {
                        if (x?.content) {
                            x.content = x.content.filter(y => y.id !== user.id);
                        }
                        return x;
                    });
                }, (error) => {
                    toastError("Failed to delete");
                    setItemLoading(user.id, false);
                    console.error(error);
                });
            }
        }
        confirmDialogReturnFunc.set(deleteConfirmed);
        showConfirmDialog.set(true);
    }

    function resetPassword(user: IUser) {
        changePasswordDialogReturnFunc.set(changePassword);
        showChangePasswordDialog.set(true);
        selectedUser = user;
    }

    function openNewUserDialog() {
        createUserDialogReturnFunc.set(createNewUser);
        showCreateUserDialog.set(true);
    }

    function createNewUser(e: any) {
        if (e?.detail?.action === "create") {
            const data = {
                username: $createUserDialog.name,
                password: $createUserDialog.password,
                isAdmin: $createUserDialog.isAdmin
            };
            const newUser = httpClient({});
            newUser.post('/users/register', data, (res) => {
                toastSuccess("Successfully created");
                allUsers.update(x => {
                    x.content.push(res);
                    return x;
                });
            }, (error) => {
                toastError("Failed to create");
                console.error(error);
            });
        }
    }

    function changePassword(e: any) {
        if (e?.detail?.action === "update") {
            setItemLoading(selectedUser.id, true);
            let updateEntry = httpClient({});
            const data = {
                username: selectedUser.name,
                isAdmin: selectedUser.isAdmin,
                password: $changePasswordDialog
            };
            updateEntry.put(`/users/password`, data, () => {
                toastSuccess("Successfully updated");
                allUsers.update(x => {
                    let index = x?.content?.findIndex(y => y.id === selectedUser.id);
                    if (index !== null && index !== -1) {
                        x.content[index].loading = false;
                    }
                    return x;
                });
            }, (error) => {
                toastError("Failed to update");
                setItemLoading(selectedUser.id, false);
                console.error(error);
            });
        }
    }

    allUsers.get(`/users`);
</script>

<div class="flex flex-row">
    <div class="text-5xl mb-4">
        All users
    </div>
    <div class="grow"></div>
    <div class="flexy">
        <div class="margins">
        <Fab color="primary" on:click={openNewUserDialog}>
            <Icon title="create new user" class="material-icons">add</Icon>
        </Fab>
        </div>
    </div>
</div>
<Card>
    <Content class="flex flex-col p-2">
        {#if $allUsers?.content?.length && $allUsers?.loading === false}
            {#each $allUsers.content as user}
            <div class="flex flex-row items-center">
                <div class="greyed-text mr-4">
                    #{user.id}
                </div>
                <div class="font-bold">
                    {user.name}
                </div>
                <div class="grow" />
                {#if user.name !== $loginName}
                    <div class="flex flex-row shrink-0">
                        <IconButton toggle pressed={user.isAdmin} class="material-icons px-0" on:click={() => changeAdminStatus(user)} readonly={user.loading}>
                            <Icon class="material-icons" title="remove admin permissions" on>security</Icon>
                            <Icon class="material-icons" title="grant admin permissions">person</Icon>
                        </IconButton>
                        <IconButton class="material-icons" title="reset password" on:click={() => resetPassword(user)}>password</IconButton>
                        <IconButton class="material-icons px-0" title="delete this user" on:click={() => deleteUser(user)}>delete</IconButton>
                    </div>
                {:else}
                    <div class="flex flex-row shrink-0">
                        <IconButton class="material-icons invisible">password</IconButton>
                    </div>
                {/if}
            </div>
            <LinearProgress indeterminate closed={!user.loading}/>
            {/each}
        {/if}
    </Content>
</Card>
