<script>
	import { isLoggedIn } from './../stores/authStore.ts';
    import { useNavigate, useLocation } from "svelte-navigator";

    const navigate = useNavigate();
    const location = useLocation();

    $: if (!$isLoggedIn) {
        navigate("/", {
            state: { from: $location.pathname },
            replace: true,
        });
    }
</script>

{#if $isLoggedIn}
    <slot />
{/if}
