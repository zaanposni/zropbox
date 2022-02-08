<script lang="ts">
	import AppSettings from './pages/AppSettings.svelte';
    import { Router, Route } from "svelte-navigator";
    import { addMessages, init, getLocaleFromNavigator } from "svelte-i18n";
    import en from "./i18n/en.json";
    import de from "./i18n/de.json";
    import Login from "./pages/Login.svelte";
    import Home from "./pages/Home.svelte";
    import { isLoggedIn, loggedInUser } from "./stores/authStore";
    import ConfirmDialog from "./components/ConfirmDialog.svelte";
    import UploadDialog from "./components/UploadDialog.svelte";
    import { SvelteToast } from '@zerodevx/svelte-toast'
    import CreateDirectoryDialog from "./components/CreateDirectoryDialog.svelte";
    import ShareEntryDialog from "./components/ShareEntryDialog.svelte";
    import Header from "./pages/Header.svelte";
    import PrivateRoute from './routes/PrivateRoute.svelte';

    addMessages("en", en);
    addMessages("en-US", en);
    addMessages("de", de);

    init({
        fallbackLocale: "en",
        initialLocale: getLocaleFromNavigator(),
    });
</script>

<style global lang="postcss">
    @tailwind base;
    @tailwind components;
    @tailwind utilities;
</style>

<Router>
    <SvelteToast />
    <ConfirmDialog />
    <UploadDialog />
    <CreateDirectoryDialog />
    <ShareEntryDialog />
    <div class="w-full h-full">
        <div class="flex flex-col flex-grow w-full h-full">
            <header>
                <nav>
                    {#if $isLoggedIn}
                        <Header />
                    {/if}
                </nav>
            </header>
            <main class="flex flex-col flex-grow w-full h-full">
                <div class="mb-4" />
                <Route path="/">
                    <Login />
                </Route>
                <PrivateRoute path="home" let:location>
                    <Home />
                </PrivateRoute>
                <PrivateRoute path="settings" let:location>
                    <AppSettings />
                </PrivateRoute>
            </main>
        </div>
    </div>
</Router>
