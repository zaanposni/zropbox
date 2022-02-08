<script lang="ts">
    import Card, { Content, Actions } from "@smui/card";
    import Button from "@smui/button";
    import { Icon } from "@smui/icon-button";
    import Textfield from "@smui/textfield";
    import LinearProgress from '@smui/linear-progress';
    import { loggedInUser } from "../stores/authStore";
    import { _ } from "svelte-i18n";
    import http from "../utils/httpClient";
    import setCookie from "../utils/setCookie";
    import { useNavigate } from "svelte-navigator";
    import getCookie from "../utils/getCookie";
    import { toastError } from "../utils/toast";

    const navigate = useNavigate();

    let username: string = "";
    let password: string = "";

    $: disabled = ((username?.trim() ?? "") === "") || ((password?.trim() ?? "") === "") || loggingInProgress;

    let loggingInProgress: boolean = false;
    let loginStore = http({});

    function login() {
        loggingInProgress = true;

        const data = {
            username,
            password
        };

        loginStore.post('/auth/login', data, (response) => {
            loggingInProgress = false;
            setCookie('zropbox_access_token', response.token, 1);
            loggedInUser.get('/auth', () => {
                navigate('/home');
            });
        }, () => {
            loggingInProgress = false;
            toastError('Login failed');
        });
    }

    const onKeyPress = e => {
        if (e.charCode === 13 && !disabled) login();
    };

    // TODO: if cookie
    if (getCookie('zropbox_access_token')) {
        loggedInUser.get('/auth', () => {
            navigate('/home');
        }, () => {
            toastError('You have been logged out');
        });
    }
</script>

<div class="flex flex-col grow justify-center items-center w-full h-full">
    <div class="card-container">
        <Card>
            <Content class="flex flex-col justify-end p-8">
                <div class="text-5xl font-medium leading-tight text-center">
                    {$_('Login.Title')}
                </div>
                <div class="p-8" on:keypress={onKeyPress}>
                    <div class="p-2">
                        <Textfield bind:value={username} label="{$_('Login.Username')}" required type="text">
                        <Icon class="material-icons mr-2" slot="leadingIcon">person</Icon>
                        </Textfield>
                    </div>
                    <div class="p-2">
                        <Textfield bind:value={password} label="{$_('Login.Password')}" required type="password">
                        <Icon class="material-icons mr-2" slot="leadingIcon">lock</Icon>
                        </Textfield>
                    </div>
                </div>
                <a class="text-sm font-medium leading-tight text-center" href="/tos.html" target="_blank">
                    {$_('Login.Guidelines')}
                </a>
            </Content>
            <Actions class="flex justify-end">
                <Button {disabled} type="submit" on:click={login}>
                    <i class="material-icons" aria-hidden="true">arrow_forward</i>{$_('Login.LoginButton')}
                </Button>
            </Actions>
            <LinearProgress indeterminate closed={!loggingInProgress}/>
        </Card>
    </div>
</div>
