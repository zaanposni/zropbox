<script lang="ts">
  import { addMessages, init, getLocaleFromNavigator } from "svelte-i18n";
  import en from "./i18n/en.json";
  import de from "./i18n/de.json";

  import Login from "./pages/Login.svelte";
  import Home from "./pages/Home.svelte";
  import { isLoggedIn, loggedInUser } from "./stores/authStore";

  addMessages("en", en);
  addMessages("en-US", en);
  addMessages("de", de);

  init({
    fallbackLocale: "en",
    initialLocale: getLocaleFromNavigator(),
  });

  loggedInUser.set({
    id: 1,
    name: "zaanposni",
  } as any);
</script>

<style global lang="postcss">
    @tailwind base;
    @tailwind components;
    @tailwind utilities;
</style>

<main class="w-full h-full">
  <div class="flex flex-grow w-full h-full">
    {#if $isLoggedIn}
      <Home></Home>
    {:else}
      <Login></Login>
    {/if}
  </div>
</main>
