<script lang="ts">
    import Card, { Content } from "@smui/card";
    import { Link } from "svelte-navigator";
    import { isAdmin, loggedInUser, loginName } from "../stores/authStore";
    import Menu, { MenuComponentDev } from "@smui/menu";
    import { Anchor } from '@smui/menu-surface';
    import List, { Item, Text, Separator, Graphic } from "@smui/list";
    import IconButton from "@smui/icon-button";
	import MediaQuery from "../components/MediaQuery.svelte";
    import { useNavigate } from "svelte-navigator";

    const navigate = useNavigate();

    let userMenu: MenuComponentDev;
    let anchor: HTMLDivElement;
    let anchorClasses: { [k: string]: boolean } = {};

    function changePassword() {
        console.log("change password");
    }
    function logout() {
        console.log("logout");
    }
    function redirect(url: string) {
        navigate(url);
    }
</script>


<Card>
    <Content class="flex flex-row p-2">
        <div class="w-1/5" />
        <div class="flex flex-row items-center w-3/5">
            <Link class="primary mr-20" to="home">Zropbox</Link>
            <MediaQuery query="(max-width: 600px)" let:matches>
                {#if $isAdmin && !matches}
                    <Link class="mr-2" to="settings">Settings</Link>
                    <Link class="mr-2" to="users">Users</Link>
                {/if}
            </MediaQuery>
            <div class="grow" />
            <div class="flex flex-row items-center">
                <IconButton class="material-icons px-0 cursor-pointer"
                    on:click={() => userMenu.setOpen(true)}>more_vert</IconButton>
                <Menu bind:this={userMenu}>
                    <List>
                        <Item class="primary cursor-default">
                            <Graphic class="material-icons cursor-default mr-2">person</Graphic>
                            <Text>{$loginName}</Text>
                        </Item>
                        <MediaQuery query="(max-width: 600px)" let:matches>
                            {#if $isAdmin && matches}
                                <Item on:SMUI:action={() => redirect('settings')}>
                                    <Graphic class="material-icons mr-2">settings</Graphic>
                                    <Text>Settings</Text>
                                </Item>
                                <Item on:SMUI:action={() => redirect('users')}>
                                    <Graphic class="material-icons mr-2">group</Graphic>
                                    <Text>Users</Text>
                                </Item>
                            {/if}
                        </MediaQuery>
                        <Item on:SMUI:action={changePassword}>
                            <Graphic class="material-icons mr-2">lock</Graphic>
                            <Text>Change password</Text>
                        </Item>
                        <Separator />
                        <Item on:SMUI:action={logout}>
                            <Graphic class="material-icons mr-2">logout</Graphic>
                            <Text>Logout</Text>
                        </Item>
                    </List>
                </Menu>
            </div>
        </div>
        <div class="w-1/5" />
    </Content>
</Card>
