<script lang="ts">
  import Card, { Content, Actions } from "@smui/card";
  import Button, { Label } from "@smui/button";
  import IconButton, { Icon } from "@smui/icon-button";
  import Textfield from "@smui/textfield";

  let username: string = "";
  let password: string = "";

  let dirtyUsername = false;
  let invalidUsername = false;
  let dirtyPassword = false;
  let invalidPassword = false;
  $: disabled = !username || !password || invalidUsername || invalidPassword;

  $: console.log(disabled, username, password, dirtyUsername, invalidUsername, dirtyPassword, invalidPassword);
</script>

<div class="flex flex-grpw justify-center items-center w-full h-full">
  <div class="card-container">
    <Card>
      <Content class="flex flex-col justify-end p-8">
        <div class="text-5xl font-medium leading-tight text-center">
            LOGIN
        </div>
        <div class="p-8">
          <div class="p-2">
            <Textfield bind:value={username} label="Username" required bind:dirty={dirtyUsername} bind:invalid={invalidUsername}>
              <Icon class="material-icons" slot="leadingIcon">person</Icon>
            </Textfield>
          </div>
          <div class="p-2">
            <Textfield bind:value={password} label="Password" required type="password" bind:dirty={dirtyPassword} bind:invalid={invalidPassword}>
              <Icon class="material-icons" slot="leadingIcon">lock</Icon>
            </Textfield>
          </div>
        </div>
      </Content>
      <Actions class="flex justify-end">
        <Button {disabled}>
          <i class="material-icons" aria-hidden="true">arrow_forward</i>LOGIN
        </Button>
      </Actions>
    </Card>
  </div>
</div>
