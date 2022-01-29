<script lang="ts">
  import Card, { Content, Actions } from "@smui/card";
  import Button from "@smui/button";
  import { Icon } from "@smui/icon-button";
  import Textfield from "@smui/textfield";
  import LinearProgress from '@smui/linear-progress';
import { loggedInUser } from "../stores/authStore";

  let username: string = "";
  let password: string = "";

  let invalidUsername = false;
  let invalidPassword = false;
  $: disabled = !username || !password || invalidUsername || invalidPassword;

  let loggingInProgress: boolean = false;

  function login() {
    loggingInProgress = true;

    setTimeout(() => {
      loggedInUser.set({} as any);
    }, 5000);
  }
</script>

<div class="flex flex-col grow justify-center items-center w-full h-full">
  <div class="card-container">
    <Card>
      <Content class="flex flex-col justify-end p-8">
        <div class="text-5xl font-medium leading-tight text-center">
            LOGIN
        </div>
        <div class="p-8">
          <div class="p-2">
            <Textfield bind:value={username} label="Username" required type="text" bind:invalid={invalidUsername} updateInvalid>
              <Icon class="material-icons mr-2" slot="leadingIcon">person</Icon>
            </Textfield>
          </div>
          <div class="p-2">
            <Textfield bind:value={password} label="Password" required type="password" bind:invalid={invalidPassword} updateInvalid>
              <Icon class="material-icons mr-2" slot="leadingIcon">lock</Icon>
            </Textfield>
          </div>
        </div>
      </Content>
      <Actions class="flex justify-end">
        <Button {disabled} type="submit" on:click={login}>
          <i class="material-icons" aria-hidden="true">arrow_forward</i>LOGIN
        </Button>
      </Actions>
      <LinearProgress indeterminate closed={! loggingInProgress}/>
    </Card>
  </div>
</div>
